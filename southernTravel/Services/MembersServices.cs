using southernTravel.Model;
using southernTravel.Repositories;

namespace southernTravel.Services
{
    // 定義介面 (Interface)
    public interface IMembersService
    {
        bool IsEmailExists(string email);
        void RegisterMember(Member member);
    }

    // 實作介面
    public class MembersServices : IMembersService
    {
        private readonly MembersRepositories _repo;

        public MembersServices(MembersRepositories repo)
        {
            _repo = repo;
        }

        public bool IsEmailExists(string email)
        {
            return _repo.CheckEmailExists(email);
        }

        public void RegisterMember(Member member)
        {
            // 你原本的邏輯
            _repo.Create(member);
        }
    }
}