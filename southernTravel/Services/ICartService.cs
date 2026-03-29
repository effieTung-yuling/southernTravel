using southernTravel.DTOs;
using southernTravel.Model;

namespace southernTravel.Services
{
    public interface ICartService
    {
        Task<CartDto?> GetCartAsync(int memberId);
        Task<CartItemDto> AddItemAsync(int memberId, CreateCartItemDto dto);
        Task<CartItemDto> UpdateItemAsync(int cartItemId, UpdateCartItemDto dto);
        Task DeleteItemAsync(int cartItemId);
    }
}
