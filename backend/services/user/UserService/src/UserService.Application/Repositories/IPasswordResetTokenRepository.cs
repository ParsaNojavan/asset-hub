using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Entities;

namespace UserService.Application.Repositories
{
    public interface IPasswordResetTokenRepository
    {
        Task CreateAsync(PasswordResetToken token);

        Task<PasswordResetToken?> GetByTokenHashAsync(string tokenHash);

        Task MarkAsUsedAsync(Guid id);

        Task DeleteByUserIdAsync(Guid userId);
    }
}
