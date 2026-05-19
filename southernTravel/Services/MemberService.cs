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
    // 宣告從 IAttractionService 介面繼承
    public class MemberService : IMemberService
    {  // 注入 IMemberRepository 和 IConfiguration （用來讀取 appsettings.json 的設定），封裝:只有在MemberService能取得資料
        private readonly IMemberRepository _memberRepository;
        private readonly IConfiguration _configuration; // 新增這行
        // 建構子 => 這裡會在程式啟動時被呼叫，並且會自動注入 IMemberRepository 和 IConfiguration 的實例
        public MemberService(IMemberRepository memberRepository, IConfiguration configuration)
        {
            _memberRepository = memberRepository;
            _configuration = configuration; // 注入進來
        }
        // 使用泛型寫法，這裡是呼叫 _memberRepository.GetAllAsync() 向資料庫拿資料，然後回傳給 Controller，最後由 Controller 包裝成 HTTP 格式送給前端。
        public async Task<List<Member>> GetAllMembersAsync()
        {
            return await _memberRepository.GetAllAsync();
        }

        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            return await _memberRepository.GetByIdAsync(id);
        }
        // 實務上 bool 代表的是「這個商業邏輯執行的結果是成功、還是失敗」
        // bool 不是用來檢查「有沒有傳資料進來」。資料有沒有傳進來，在參數（例如 Member member 或 int id）進來時就決定了。
        public async Task<bool> RegisterMemberAsync(Member member)
        {
            // 先檢查 Email 是否有人用了。
            // if (emailExists) return false; $\rightarrow$ 意思是：「因為 Email 重複了，所以我判定『註冊失敗』，丟回 false 給 Controller 知道。」
            var emailExists = await _memberRepository.CheckEmailExistsAsync(member.Email);

            if (emailExists)
            {
                return false;
            }
            // 如果沒重複，順利存入資料庫，return true; $\rightarrow$ 意思是：「註冊成功！」
            await _memberRepository.CreateAsync(member);
            return true;
        }

        public async Task<bool> UpdateMemberAsync(int id, UpdateMemberRequest request)
        {
            // 先用 id 去資料庫找人。
            var member = await _memberRepository.GetByIdAsync(id);
            // if (member == null) return false; $\rightarrow$ 意思是：「靠，資料庫根本沒有這個人，沒辦法改/刪，所以我判定『執行失敗』，丟回 false。」
            if (member == null)
            {
                return false;
            }

            member.Name = request.Name;
            member.PhoneNumber = request.PhoneNumber;
            member.Birthday = request.Birthday;
            member.UpdatedAt = DateTime.UtcNow;
            // 順利改完/刪完，return true; $\rightarrow$ 意思是：「修改/刪除成功！」
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
        // 是不是泛型？ 不是，Task<LoginResponseDto?> 裡面的 ? 代表「可空型別（Nullable）」，意思是可以回傳一個 DTO 物件，或者回傳 null。它不是泛型。
        // LoginResponseDto 是前端需要的格式嗎？ 完全正確！ 前端登入成功後，需要把 Token 存起來，並在畫面上顯示「歡迎，XXX」，所以這個 DTO 裡裝了 Token、Name、Email，非常標準。
        // 驗證通過 往下執行，產生 JWT Token，把資料打包進 LoginResponseDto 回傳給 Controller。
        public async Task<LoginResponseDto?> LoginAsync(string email, string password)
        {
            // 呼叫 GetByEmailAsync(email) 沒錯，先去資料庫找有沒有這個 Email 的會員。
            var member = await _memberRepository.GetByEmailAsync(email);
            // BCrypt.Verify(...) 沒錯，比對密碼對不對。
            // 如果帳號不存在或密碼錯，回傳 null（告訴 Controller 登入失敗）。
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