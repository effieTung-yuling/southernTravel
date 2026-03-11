using southernTravel.Model;

namespace southernTravel.Repositories
{
    public interface IMemberRepository
    {
        Task<List<Member>> GetAllAsync();

        Task<Member?> GetByIdAsync(int id);

        Task<bool> CheckEmailExistsAsync(string email);

        Task CreateAsync(Member member);
    }
}