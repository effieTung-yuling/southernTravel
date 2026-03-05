using southernTravel.Model;
using southernTravel.Services;
using Microsoft.AspNetCore.Mvc;

namespace southernTravel.Controllers
{
    [ApiController]
    [Route("api/[Controller]")] // 建議統一用 api/ 開頭
    public class MembersController : ControllerBase // 必須繼承 ControllerBase 才能使用 Ok() 或 BadRequest()
    {
        private readonly MembersServices _service;

        // 注入你的 Service
        public MembersController(MembersServices service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null) return BadRequest("資料不能為空");

            try
            {
                // 1. 檢查 Email 是否重複 (交給 Service 去處理)
                if (_service.IsEmailExists(request.Email))
                {
                    return BadRequest("該 Email 已經被註冊過了。");
                }

                // 2. 建立新會員物件 (將 DTO 轉為實體 Model)
                var newMember = new Member
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = request.Password, // 暫時明碼存儲
                    PhoneNumber = request.PhoneNumber,
                    Birthday = request.Birthday,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                // 3. 呼叫 Service 存入資料庫
                _service.RegisterMember(newMember);

                return Ok(new { message = "註冊成功！", userId = newMember.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"伺服器錯誤：{ex.Message}");
            }
        }
    }
}