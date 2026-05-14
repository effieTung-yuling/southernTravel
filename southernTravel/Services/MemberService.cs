using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using southernTravel.DTOs;
using southernTravel.Model;
using southernTravel.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace southernTravel.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IConfiguration _configuration; // 新增這行

        public MemberService(IMemberRepository memberRepository, IConfiguration configuration)
        {
            _memberRepository = memberRepository;
            _configuration = configuration; // 注入進來
        }

        public async Task<List<Member>> GetAllMembersAsync()
        {
            return await _memberRepository.GetAllAsync();
        }

        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            return await _memberRepository.GetByIdAsync(id);
        }

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

        public async Task<LoginResponseDto?> LoginAsync(string email, string password)
        {
            var member = await _memberRepository.GetByEmailAsync(email);

            if (member == null || !BCrypt.Net.BCrypt.Verify(password, member.PasswordHash))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            // 改成從 appsettings.json 讀取
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? _configuration["JwtSettings:JWT_KEY"];
            // 如果連 appsettings 都沒設定，才報錯
            if (string.IsNullOrEmpty(jwtKey)) throw new Exception("設定檔中找不到 JwtSettings:JWT_KEY");

            var key = Encoding.UTF8.GetBytes(jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
                    new Claim(ClaimTypes.Email, member.Email),
                    new Claim(ClaimTypes.Name, member.Name),
                    new Claim(ClaimTypes.Role, member.MemberType)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                MemberType = member.MemberType
            };
        }
    }
}