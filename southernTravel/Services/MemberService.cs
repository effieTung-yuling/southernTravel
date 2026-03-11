using southernTravel.DTOs;
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
        // 更新會員資料
        public async Task<bool> UpdateMemberAsync(int id, UpdateMemberRequest request)
        {
            var member = await _memberRepository.GetByIdAsync(id);

            if (member == null)
            {
                return false;
            }

            member.Name = request.Name;
            member.PhoneNumber = request.PhoneNumber;
            member.Birthday = request.Birthday;
            member.UpdatedAt = DateTime.UtcNow;

            await _memberRepository.UpdateAsync(member);

            return true;
        }

        // 刪除會員
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
    }
}