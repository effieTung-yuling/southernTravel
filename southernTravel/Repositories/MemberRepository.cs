using Microsoft.EntityFrameworkCore;
using southernTravel.Data;
using southernTravel.Model;

namespace southernTravel.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDbContext _context;

        public MemberRepository(AppDbContext context)
        {
            _context = context;
        }

        // 取得所有會員
        public async Task<List<Member>> GetAllAsync()
        {
            return await _context.Members.ToListAsync();
        }

        // 取得單一會員
        public async Task<Member?> GetByIdAsync(int id)
        {
            return await _context.Members
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        // 檢查 Email 是否存在
        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _context.Members
                .AnyAsync(m => m.Email == email);
        }

        // 建立會員
        public async Task CreateAsync(Member member)
        {
            await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();
        }
        // 更新會員資料
        public async Task UpdateAsync(Member member)
        {
            _context.Members.Update(member);
            await _context.SaveChangesAsync();
        }

        // 刪除會員
        public async Task DeleteAsync(Member member)
        {
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
        }
    }
}