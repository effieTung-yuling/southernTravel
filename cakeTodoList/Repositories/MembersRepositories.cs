using southernTravel.Data;
using southernTravel.Model;
namespace southernTravel.Repositories
{
    public class MembersRepositories
    {
        private readonly AppDbContext _context;
        public MembersRepositories(AppDbContext context)
        {
            _context = context;
        }

        // 檢查 Email 是否重複
        public bool CheckEmailExists(string email)
        {
            return _context.Members.Any(m => m.Email == email);
        }

        // 執行資料庫寫入
        public void Create(Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges();
        }
    }
}
