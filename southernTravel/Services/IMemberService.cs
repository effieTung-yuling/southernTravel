using southernTravel.Model;

namespace southernTravel.Services
{
    public interface IMemberService
    {
        Task<List<Member>> GetAllMembersAsync();

        Task<Member?> GetMemberByIdAsync(int id);

        Task<bool> RegisterMemberAsync(Member member);
    }
}