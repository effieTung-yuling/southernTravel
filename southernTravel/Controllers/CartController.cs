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
            int memberId;
            try { memberId = GetMemberIdFromToken(); }
            catch (UnauthorizedAccessException ex) { return Unauthorized(new { message = ex.Message }); }

            var cart = await _service.GetCartAsync(memberId);
            if (cart == null)
                return NotFound(new { message = $"編號 {memberId} 的會員目前沒有購物車資料。" });

            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] CreateCartItemDto dto)
        {
            int memberId;
            try { memberId = GetMemberIdFromToken(); }
            catch (UnauthorizedAccessException ex) { return Unauthorized(new { message = ex.Message }); }

            var item = await _service.AddItemAsync(memberId, dto);
            if (item == null)
                return NotFound(new { message = $"找不到 ID 為 {dto.ProductId} 的產品資料。" });

            return Ok(item);
        }

        [HttpPut("items/{itemId}")]
        public async Task<IActionResult> UpdateItem(int itemId, [FromBody] UpdateCartItemDto dto)
        {
            var item = await _service.UpdateItemAsync(itemId, dto);
            if (item == null)
                return NotFound(new { message = $"找不到 ID 為 {itemId} 的購物車項目。" });

            return Ok(item);
        }

        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            var success = await _service.DeleteItemAsync(itemId);
            if (!success)
                return NotFound(new { message = $"找不到 ID 為 {itemId} 的購物車項目。" });

            return NoContent();
        }
    }
}
