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
        public async Task<IActionResult> UpdateItem(int? itemId, [FromBody] UpdateCartItemDto dto)
        {
            if (itemId == null || itemId <= 0)
                return BadRequest(new { message = "itemId 為必填且必須為正整數。" });

            // 驗證 Token
            int memberId;
            try { memberId = GetMemberIdFromToken(); }
            catch (UnauthorizedAccessException ex) { return Unauthorized(new { message = ex.Message }); }

            // 驗證 DTO（ProductId、Qty）由 [ApiController] + DataAnnotations 自動處理
            // 但若未套用預設行為，可手動檢查：
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = await _service.UpdateItemAsync(itemId.Value, dto);

            // 區分「找不到購物車項目」與「ProductId 不符」
            if (item == null)
            {
                // 嘗試判斷是否為 ProductId 不符
                return NotFound(new
                {
                    message = $"找不到 ID 為 {itemId} 的購物車項目，或提供的 ProductId（{dto.ProductId}）與項目不符，無法修改數量。"
                });
            }

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
