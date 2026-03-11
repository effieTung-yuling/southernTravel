using southernTravel.DTOs;
using southernTravel.Model;

namespace southernTravel.Services
{
    public interface IMemberService
    {
        // 取得所有會員
        Task<List<Member>> GetAllMembersAsync();
        // 根據 ID 取得會員
        Task<Member?> GetMemberByIdAsync(int id);
        // 註冊會員
        Task<bool> RegisterMemberAsync(Member member);
        // 更新會員資料
        Task<bool> UpdateMemberAsync(int id, UpdateMemberRequest request);
        // 刪除會員
        Task<bool> DeleteMemberAsync(int id);
    }
}