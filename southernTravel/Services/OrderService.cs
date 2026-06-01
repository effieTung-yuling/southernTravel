using southernTravel.DTOs;
using southernTravel.Model;
using southernTravel.Data;
using southernTravel.Repositories;
using Microsoft.EntityFrameworkCore;

namespace southernTravel.Services
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OrdersDto>> GetAllAsync()
        {
            // 1. 撈資料
            var orders = await _repository.GetAllAsync();

            // 2. 防呆
            if (orders == null || !orders.Any())    
                return null;

            // 3. Model → DTO
            return orders.Select(order => new OrdersDto
            {
                OrderNo = order.OrderNo,
                Name = order.Name
            }).ToList();
        }
    }
}
