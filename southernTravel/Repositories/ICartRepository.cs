using southernTravel.Model;

namespace southernTravel.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByMemberIdAsync(int memberId);
        Task<CartItem?> GetCartItemAsync(int cartItemId);
        Task AddCartItemAsync(CartItem item);
        Task UpdateCartItemAsync(CartItem item);
        Task DeleteCartItemAsync(CartItem item);
        Task<CartItem?> GetCartItemWithCartAsync(int cartItemId);
    }
}
