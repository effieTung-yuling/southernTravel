using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using southernTravel.DTOs;
using southernTravel.Model;
using southernTravel.Services;
using System.ComponentModel.DataAnnotations;

namespace southernTravel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _service;
        private readonly IValidator<RegisterRequest> _validator;
        public MemberController(IMemberService service, IValidator<RegisterRequest> validator)
        {
            _service = service;
            _validator = validator;
        }

        // 取出所有會員
        [HttpGet] // GET api/Member
        public async Task<IActionResult> GetAllMembers()
        {
            try
            {
                var members = await _service.GetAllMembersAsync();

                return Ok(members);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"查詢失敗：{ex.Message}");
            }
        }

        // 取出單一會員
        [HttpGet("{id}")] // 這裡定義了路徑參數，例如：api/Member/5
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var member = await _service.GetMemberByIdAsync(id);

                if (member == null)
                {
                    return NotFound($"找不到 ID 為 {id} 的會員");
                }

                // 安全提醒：回傳時建議不要包含 PasswordHash
                return Ok(new
                {
                    member.Id,
                    member.Name,
                    member.Email,
                    member.PhoneNumber,
                    member.Birthday,
                    member.CreatedAt
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"查詢失敗：{ex.Message}");
            }
        }
        // 註冊會員
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null) return BadRequest("資料不能為空");

            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    errors = validationResult.Errors.Select(e => new
                    {
                        e.PropertyName,
                        e.ErrorMessage
                    })
                });
            }

            try
            {
                var newMember = new Member
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = request.Password,
                    PhoneNumber = request.PhoneNumber,
                    Birthday = request.Birthday.Value,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _service.RegisterMemberAsync(newMember);

                if (!result)
                {
                    return BadRequest("該 Email 已經被註冊過了");
                }

                return Ok(new
                {
                    message = "註冊成功",
                    userId = newMember.Id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"伺服器錯誤：{ex.Message}");
            }
        }

        // 更新會員資料
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] UpdateMemberRequest request)
        {
            try
            {
                var result = await _service.UpdateMemberAsync(id, request);

                if (!result)
                {
                    return NotFound($"找不到 ID 為 {id} 的會員");
                }

                return Ok(new { message = "會員資料更新成功" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"更新失敗：{ex.Message}");
            }
        }

        // 刪除會員
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            try
            {
                var result = await _service.DeleteMemberAsync(id);

                if (!result)
                {
                    return NotFound($"找不到 ID 為 {id} 的會員");
                }

                return Ok(new { message = "會員刪除成功" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"刪除失敗：{ex.Message}");
            }
        }
    }
}