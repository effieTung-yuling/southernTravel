using Microsoft.AspNetCore.Mvc;
using southernTravel.DTOs;
using southernTravel.Services;

namespace southernTravel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController: Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            if (orders == null || !orders.Any())
                return NotFound("No orders found.");

            return Ok(orders);
        }
    }
}
