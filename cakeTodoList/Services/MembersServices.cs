using southernTravel.Model;
using southernTravel.Repositories;

namespace southernTravel.Services
{
    public class MembersServices
    {
        private readonly MembersRepositories _repo;

        public MembersServices(MembersRepositories repo)
        {
            _repo = repo;
        }

        // --- 補上這一段，錯誤就會消失 ---
        public bool IsEmailExists(string email)
        {
            // 轉手交給 Repository 去查資料庫
            return _repo.CheckEmailExists(email);
        }
        // ----------------------------

        public void RegisterMember(Member member)
        {
            if (_repo.CheckEmailExists(member.Email))
            {
                throw new Exception("Email 已存在");
            }
            _repo.Create(member);
        }
    }
}