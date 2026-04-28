using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Repositories;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Repositories
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly IDbConnection _db;

        public PasswordResetTokenRepository(IDbConnection db)
        {
            _db = db;
        }
        public async Task CreateAsync(PasswordResetToken token)
        {
            var sql = @"INSERT INTO PasswordResetTokens
                        (Id, UserId, TokenHash, ExpiresAt, Used, CreatedAt)
                        VALUES
                        (@Id, @UserId, @TokenHash, @ExpiresAt, @Used, @CreatedAt)";

            await _db.ExecuteAsync(sql, token);
        }

        public async Task DeleteByUserIdAsync(Guid userId)
        {
            var sql = @"DELETE FROM PasswordResetTokens
                        WHERE UserId = @UserId";

            await _db.ExecuteAsync(sql, new { UserId = userId });
        }

        public async Task<PasswordResetToken?> GetByTokenHashAsync(string tokenHash)
        {
            var sql = @"SELECT *
                        FROM PasswordResetTokens
                        WHERE TokenHash = @TokenHash";

            return await _db.QueryFirstOrDefaultAsync<PasswordResetToken>(
                sql,
                new { TokenHash = tokenHash });
        }

        public async Task MarkAsUsedAsync(Guid id)
        {
            var sql = @"UPDATE PasswordResetTokens
                        SET Used = 1
                        WHERE Id = @Id";

            await _db.ExecuteAsync(sql, new { Id = id });
        }
    }
}
