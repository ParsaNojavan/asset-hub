using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Repositories
{
    public interface IUserRepository
    {
        Task InsertUser(User user);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(Guid id);
        Task DeleteUser(Guid userId);
        Task UpdateUser(User user);
        Task<IEnumerable<User>> SearchUsersAsync(string query);

    }
}
