using southernTravel.DTOs;
using southernTravel.Model;

namespace southernTravel.Services
{
    public interface ICartService
    {
        Task<Cart?> GetCartAsync(int memberId);
        Task<CartItem?> AddItemAsync(int memberId, CreateCartItemDto dto);
        Task<CartItem?> UpdateItemAsync(int cartItemId, UpdateCartItemDto dto);
        Task DeleteItemAsync(int cartItemId);
    }
}
