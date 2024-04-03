using Mock1Trainning.Models;
using Mock1Trainning.Models.DTO;

namespace Mock1Trainning.Repository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string userName);
        Task<LocalUser> Login(string userName, string passWord);
        Task<LocalUser> Register(LocalUser user);
    }
}
