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
        Task<LoginResponseDto?> LoginAsync(string email, string password);
    }
}