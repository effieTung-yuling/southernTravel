using southernTravel.Data;
using southernTravel.DTOs;
using southernTravel.Model;
using southernTravel.Repositories;

namespace southernTravel.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repo;
        private readonly AppDbContext _context;
        public CartService(ICartRepository repo, AppDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<Cart?> GetCartAsync(int memberId)
        {
            return await _repo.GetCartByMemberIdAsync(memberId);
        }

        public async Task<CartItem?> AddItemAsync(int memberId, CreateCartItemDto dto)
        {
            var cart = await _repo.GetCartByMemberIdAsync(memberId);

            if (cart == null)
            {
                cart = new Cart { MemberId = memberId };
                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();
            }

            var item = new CartItem
            {
                CartId = cart.Id,
                ProductId = dto.ProductId,
                Qty = dto.Qty,
                Price = dto.Price,
                Total = dto.Price * dto.Qty
            };

            await _repo.AddCartItemAsync(item);

            // ⭐ 重新抓 cart（重點！）
            cart = await _repo.GetCartByMemberIdAsync(memberId);

            cart!.FinalTotal = cart.CartItems.Sum(ci => ci.Total);
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

            cart!.FinalTotal = cart.CartItems.Sum(ci => ci.Total);
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

            cart!.FinalTotal = cart.CartItems.Sum(ci => ci.Total);
            await _context.SaveChangesAsync();
        }
    }
}
