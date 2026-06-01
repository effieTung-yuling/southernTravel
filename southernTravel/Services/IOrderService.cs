using southernTravel.DTOs;
namespace southernTravel.Services
{
    public interface IOrderService
    {
        Task<List<OrdersDto>> GetAllAsync();
    }
}
