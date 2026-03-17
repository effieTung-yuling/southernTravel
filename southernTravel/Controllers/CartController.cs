using Microsoft.AspNetCore.Mvc;
using southernTravel.DTOs;
using southernTravel.Services;

namespace southernTravel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;
        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpGet("{memberId}")]
        public async Task<IActionResult> GetCart(int memberId)
        {
            var cart = await _service.GetCartAsync(memberId);
            return Ok(cart);
        }

        [HttpPost("{memberId}/items")]
        public async Task<IActionResult> AddItem(int memberId, [FromBody] CreateCartItemDto dto)
        {
            var item = await _service.AddItemAsync(memberId, dto);
            return Ok(item);
        }

        [HttpPut("items/{itemId}")]
        public async Task<IActionResult> UpdateItem(int itemId, [FromBody] UpdateCartItemDto dto)
        {
            var item = await _service.UpdateItemAsync(itemId, dto);
            return Ok(item);
        }

        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            await _service.DeleteItemAsync(itemId);
            return NoContent();
        }
    }
}
