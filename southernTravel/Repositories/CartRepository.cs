using southernTravel.Data;
using southernTravel.Model;
using Microsoft.EntityFrameworkCore;

namespace southernTravel.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;
        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartByMemberIdAsync(int memberId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.MemberId == memberId);
        }

        public async Task<CartItem?> GetCartItemAsync(int cartItemId)
        {
            return await _context.CartItems.FindAsync(cartItemId);
        }

        public async Task AddCartItemAsync(CartItem item)
        {
            await _context.CartItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(CartItem item)
        {
            _context.CartItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartItemAsync(CartItem item)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<CartItem?> GetCartItemWithCartAsync(int cartItemId)
        {
            return await _context.CartItems
                .Include(ci => ci.Cart)
                .ThenInclude(c => c.CartItems)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
        }
    }
}
