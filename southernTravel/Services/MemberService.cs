using southernTravel.DTOs;
using southernTravel.Model;
using southernTravel.Repositories;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt; // 記得引用這個
using Microsoft.IdentityModel.Tokens;

namespace southernTravel.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        // 取得所有會員
        public async Task<List<Member>> GetAllMembersAsync()
        {
            return await _memberRepository.GetAllAsync();
        }

        // 取得單一會員
        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            return await _memberRepository.GetByIdAsync(id);
        }

        // 註冊會員
        public async Task<bool> RegisterMemberAsync(Member member)
        {
            var emailExists = await _memberRepository.CheckEmailExistsAsync(member.Email);

            if (emailExists)
            {
                return false;
            }

            await _memberRepository.CreateAsync(member);

            return true;
        }
        // 更新會員資料
        public async Task<bool> UpdateMemberAsync(int id, UpdateMemberRequest request)
        {
            var member = await _memberRepository.GetByIdAsync(id);

            if (member == null)
            {
                return false;
            }

            member.Name = request.Name;
            member.PhoneNumber = request.PhoneNumber;
            member.Birthday = request.Birthday;
            member.UpdatedAt = DateTime.UtcNow;

            await _memberRepository.UpdateAsync(member);

            return true;
        }

        // 刪除會員
        public async Task<bool> DeleteMemberAsync(int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);

            if (member == null)
            {
                return false;
            }

            await _memberRepository.DeleteAsync(member);

            return true;
        }

        // 根據 Email 取得會員
        // 登入會員
        public async Task<string?> LoginAsync(string email, string password)
        {
            // 1. 透過 Repo 去 Neon 資料庫撈資料
            var member = await _memberRepository.GetByEmailAsync(email);

            // 2. 驗證 (目前先比對明碼，之後強烈建議改雜湊)
            if (member == null || member.PasswordHash != password)
            {
                return null;
            }

            // 3. 產生 JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();

            // 從 Render 環境變數抓 Key
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            if (string.IsNullOrEmpty(jwtKey)) throw new Exception("Render 環境變數未設定 JWT_KEY");

            var key = Encoding.UTF8.GetBytes(jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
                    new Claim(ClaimTypes.Email, member.Email),
                    new Claim(ClaimTypes.Name, member.Name)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}