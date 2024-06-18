using NadinSoftTask.Domain.Entities.DTO;
using NadinSoftTask.Domain.Repository;
using NadinSoftTask.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NadinSoftTask.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool isUniqueUser(string username)
        {
            var user = _db.LocalUsers.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.LocalUsers.FirstOrDefault(u =>u.Username.ToLower() == loginRequestDTO.Username.ToLower()
                && u.Password == loginRequestDTO.Password);
            if (user == null)
            {
                return null;
            }

            //if user found generate JWT Token

        }

        public async Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            LocalUser user = new LocalUser()
            {
                Name = registerationRequestDTO.Name,
                Username = registerationRequestDTO.Username,
                Password = registerationRequestDTO.Password,
                Role = registerationRequestDTO.Role,
            };
            await _db.LocalUsers.AddAsync(user);
            await _db.SaveChangesAsync();
            user.Password = "";
            return user;

        }
    }
}
