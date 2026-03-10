using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using southernTravel.Model;
using southernTravel.Services;
using System.ComponentModel.DataAnnotations;

namespace southernTravel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMembersService _service;
        private readonly IValidator<RegisterRequest> _validator;
        public UserController(IMembersService service, IValidator<RegisterRequest> validator)
        {
            _service = service;
            _validator = validator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // 2. 基本 null 檢查
            if (request == null) return BadRequest("資料不能為空");

            // 3. 格式校驗 (Email格式、電話、密碼大寫等)
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                // 將 FluentValidation 的錯誤訊息轉成 API 格式回傳
                return BadRequest(new
                {
                    errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                });
            }

            try
            {
                // 4. 業務邏輯檢查 (Email 是否重複)
                if (_service.IsEmailExists(request.Email))
                {
                    return BadRequest("該 Email 已經被註冊過了。");
                }

                // 5. 實體轉換與存檔
                var newMember = new Member
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = request.Password, // 提醒：實務上建議使用 Hash 處理
                    PhoneNumber = request.PhoneNumber,
                    Birthday = request.Birthday.Value,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

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