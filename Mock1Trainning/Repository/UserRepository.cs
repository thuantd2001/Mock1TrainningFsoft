using Microsoft.EntityFrameworkCore;
using Mock1Trainning.Data;
using Mock1Trainning.Models;
using Mock1Trainning.Models.DTO;

namespace Mock1Trainning.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

       
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
           
        }

        public bool IsUniqueUser(string userName)
        {
            var user = _context.LocalUsers.FirstOrDefault(l => l.UserName.Equals(userName));
            if(user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LocalUser> Login(string userName, string passWord)
        {
            var  user = await _context.LocalUsers.FirstOrDefaultAsync(u => u.UserName.Equals(userName) && u.Password.Equals(passWord));
           
          return user;

        }

        public async Task<LocalUser> Register(LocalUser user)
        {
            await _context.LocalUsers.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
