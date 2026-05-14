using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using southernTravel.DTOs;
using southernTravel.Services;
using System.Security.Claims;

namespace southernTravel.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        private int GetMemberIdFromToken()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? User.FindFirst("memberId")?.Value;

            if (claim == null || !int.TryParse(claim, out var memberId))
                throw new UnauthorizedAccessException("無法從 Token 取得會員 ID");

            return memberId;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var memberId = GetMemberIdFromToken();
            var cart = await _service.GetCartAsync(memberId);
            if (cart == null)
            {
                return NotFound(new
                {
                    status = 404,
                    message = $"查詢失敗：編號 {memberId} 的會員目前沒有購物車資料。"
                });
            }

            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] CreateCartItemDto dto)
        {
            var memberId = GetMemberIdFromToken();
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
