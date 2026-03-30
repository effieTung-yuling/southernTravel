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
            if (cart == null)
            {
                // 這裡就是改掉 204 的地方，改回傳 404 並自訂訊息
                return NotFound(new
                {
                    status = 404,
                    message = $"查詢失敗：編號 {memberId} 的會員目前沒有購物車資料。"
                });
            }

            return Ok(cart);
        }

        [HttpPost("{memberId}/items")]
        public async Task<IActionResult> AddItem(int memberId, [FromBody] CreateCartItemDto dto)
        {
            var item = await _service.AddItemAsync(memberId, dto);
            return Ok(dto);
        }

        [HttpPut("items/{itemId}")]
        public async Task<IActionResult> UpdateItem(int itemId, [FromBody] UpdateCartItemDto dto)
        {
            var item = await _service.UpdateItemAsync(itemId, dto);
            return Ok(dto);
        }

        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            await _service.DeleteItemAsync(itemId);
            return NoContent();
        }
    }
}
