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
                    ProductName = ci.Product?.Title, // ⭐ 記得 Include Product
                    Qty = ci.Qty,
                    Price = ci.Price,
                    Total = ci.Total
                }).ToList()
            };
        }

        public async Task<CartItem?> AddItemAsync(int memberId, CreateCartItemDto dto)
        {
            // 1. 找商品
            var product = await _productRepository.GetProductByIdAsync(dto.ProductId);
            if (product == null) throw new Exception("Product not found");

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

            // 3. 建立 CartItem
            var item = new CartItem
            {
                CartId = cart.Id,
                ProductId = dto.ProductId,
                Qty = dto.Qty,
                Price = product.Price,                 // ✅ 從 DB 拿
                Total = dto.Qty * product.Price        // ✅ 後端算
            };

            await _repo.AddCartItemAsync(item);

            // 重新抓最新 cart（保證正確）
            var updatedCart = await _repo.GetCartByMemberIdAsync(memberId);

            updatedCart!.FinalTotal = await _context.CartItems
            .Where(ci => ci.CartId == updatedCart.Id)
            .SumAsync(ci => ci.Total);

            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<CartItem?> UpdateItemAsync(int cartItemId, UpdateCartItemDto dto)
        {
            var item = await _repo.GetCartItemWithCartAsync(cartItemId);
            if (item == null) throw new Exception("CartItem not found");

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

            return item;
        }

        public async Task DeleteItemAsync(int cartItemId)
        {
            var item = await _repo.GetCartItemWithCartAsync(cartItemId);
            if (item == null) throw new Exception("CartItem not found");

            var memberId = item.Cart!.MemberId;

            await _repo.DeleteCartItemAsync(item);

            // ⭐ 重新抓 cart
            var cart = await _repo.GetCartByMemberIdAsync(memberId);

            cart!.FinalTotal = await _context.CartItems
            .Where(ci => ci.CartId == cart.Id)
            .SumAsync(ci => ci.Total);
            await _context.SaveChangesAsync();
        }
    }
}
