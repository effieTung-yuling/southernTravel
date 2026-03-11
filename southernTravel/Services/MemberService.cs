using southernTravel.Model;
using southernTravel.Repositories;

namespace southernTravel.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        // 取得所有會員
        public async Task<List<Member>> GetAllMembersAsync()
        {
            return await _memberRepository.GetAllAsync();
        }

        // 取得單一會員
        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            return await _memberRepository.GetByIdAsync(id);
        }

        // 註冊會員
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
    }
}