using southernTravel.DTOs;
using southernTravel.Model;

namespace southernTravel.Services
{
    public interface IMemberService
    {
        Task<List<Member>> GetAllMembersAsync();
        Task<Member?> GetMemberByIdAsync(int id);
        Task<bool> RegisterMemberAsync(Member member);
        Task<bool> UpdateMemberAsync(int id, UpdateMemberRequest request);
        Task<bool> DeleteMemberAsync(int id);

        // 修正：登入成功應該回傳 Token 字串，失敗回傳 null
        Task<string?> LoginAsync(string email, string password);
    }
}