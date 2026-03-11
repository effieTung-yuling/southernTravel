using southernTravel.Model;

namespace southernTravel.Repositories
{
    public interface IMemberRepository
    {   
        // 取得所有會員
        Task<List<Member>> GetAllAsync();
        // 根據 ID 取得會員
        Task<Member?> GetByIdAsync(int id);
        // 檢查 Email 是否已存在
        Task<bool> CheckEmailExistsAsync(string email);
        // 新增會員
        Task CreateAsync(Member member);
        // 修改會員資料
        Task UpdateAsync(Member member);
        // 刪除會員
        Task DeleteAsync(Member member);
    }
}