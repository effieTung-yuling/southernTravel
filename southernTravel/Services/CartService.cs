using Microsoft.EntityFrameworkCore;
using southernTravel.Data;
using southernTravel.DTOs;
using southernTravel.Model;
using southernTravel.Repositories;

namespace southernTravel.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repo;
        private readonly IProductRepository _productRepository; 
        private readonly AppDbContext _context;
        public CartService(ICartRepository repo,
    IProductRepository productRepository, AppDbContext context)
        {
            _repo = repo;
            _productRepository = productRepository;
            _context = context;
        }

        public async Task<CartDto?> GetCartAsync(int memberId)
        {
            var cart = await _repo.GetCartByMemberIdAsync(memberId);

            if (cart == null) return null;

            return new CartDto
            {
                Id = cart.Id,
                MemberId = cart.MemberId,
                FinalTotal = cart.FinalTotal,
                Items = cart.CartItems.Select(ci => new CartItemDto
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Title,
                    Qty = ci.Qty,
                    Price = ci.Price,
                    Total = ci.Total
                }).ToList()
            };
        }

        public async Task<CartItemDto?> AddItemAsync(int memberId, CreateCartItemDto dto)
        {
            // 1. 找商品
            var product = await _productRepository.GetProductByIdAsync(dto.ProductId);
            if (product == null) return null;

            // 2. 找或建立 Cart
            var cart = await _repo.GetCartByMemberIdAsync(memberId);
            if (cart == null)
            {
                cart = new Cart
                {
                    MemberId = memberId
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            // 3. 建立或累加 CartItem
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);
            CartItem item;

            if (existingItem != null)
            {
                // 已有相同商品 → 累加數量
                existingItem.Qty += dto.Qty;
                existingItem.Total = existingItem.Qty * existingItem.Price;
                await _repo.UpdateCartItemAsync(existingItem);
                item = existingItem;
            }
            else
            {
                // 新商品 → 新增一筆
                item = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = dto.ProductId,
                    Qty = dto.Qty,
                    Price = product.Price,
                    Total = dto.Qty * product.Price
                };
                await _repo.AddCartItemAsync(item);
            }

            // 重新抓最新 cart（保證正確）
            var updatedCart = await _repo.GetCartByMemberIdAsync(memberId);

            updatedCart!.FinalTotal = await _context.CartItems
            .Where(ci => ci.CartId == updatedCart.Id)
            .SumAsync(ci => ci.Total);

            await _context.SaveChangesAsync();

            return new CartItemDto
            {
                ProductId = item.ProductId,
                ProductName = product.Title,
                Qty = item.Qty,
                Price = item.Price,
                Total = item.Total
            };
        }

        public async Task<CartItemDto?> UpdateItemAsync(int cartItemId, UpdateCartItemDto dto)
        {
            var item = await _repo.GetCartItemWithCartAsync(cartItemId);
            if (item == null) return null;
            if(dto.ProductId != item.ProductId) return null; // 確保 productId 不變
            item.Qty = dto.Qty;
            item.Total = item.Qty * item.Price;

            await _repo.UpdateCartItemAsync(item);

            // ⭐ 安全拿 memberId
            var memberId = item.Cart!.MemberId;

            // ⭐ 重新抓 cart
            var cart = await _repo.GetCartByMemberIdAsync(memberId);

            cart!.FinalTotal = await _context.CartItems
                .Where(ci => ci.CartId == cart.Id)
                .SumAsync(ci => ci.Total);
            await _context.SaveChangesAsync();

            return new CartItemDto
            {
                ProductId = item.ProductId,
                ProductName = item.Product?.Title,
                Qty = item.Qty,
                Price = item.Price,
                Total = item.Total
            };
        }

        public async Task<bool> DeleteItemAsync(int cartItemId)
        {
            var item = await _repo.GetCartItemWithCartAsync(cartItemId);
            if (item == null) return false;

            var memberId = item.Cart!.MemberId;

            await _repo.DeleteCartItemAsync(item);

            // ⭐ 重新抓 cart
            var cart = await _repo.GetCartByMemberIdAsync(memberId);

            cart!.FinalTotal = await _context.CartItems
            .Where(ci => ci.CartId == cart.Id)
            .SumAsync(ci => ci.Total);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
