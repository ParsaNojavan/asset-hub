using AuthService.Domain.Entities;
using AuthService.Application.Repositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IDbConnection _db;
        public RefreshTokenRepository(IDbConnection db)
        {
            _db = db;
        }
        public async Task AddAsync(RefreshToken token)
        {
            token.SetCreated();

            var sql = @"INSERT INTO RefreshTokens
                        (Id,UserId,Token,ExpiresAt,IsRevoked,CreatedAt,UpdatedAt)
                        VALUES
                        (@Id,@UserId,@Token,@ExpiresAt,@IsRevoked,@CreatedAt,@UpdatedAt)";

            await _db.ExecuteAsync(sql, token);
        }

        public async Task DeleteByUserId(Guid userId)
        {
            var sql = "DELETE FROM RefreshTokens WHERE UserId = @UserId";
            await _db.ExecuteAsync(sql, new {UserId = userId});
        }

        public async Task<RefreshToken?> GetAsync(string token)
        {
            var sql = "SELECT * FROM RefreshTokens WHERE Token = @Token";
            return await _db.QueryFirstOrDefaultAsync<RefreshToken>(sql, new { Token = token });
        }

        public async Task RevokeAsync(string token)
        {
            var sql = @"UPDATE RefreshTokens
                        SET IsRevoked = 1, UpdatedAt = GETUTCDATE()
                        WHERE Token = @Token";

            await _db.ExecuteAsync(sql, new {Token = token});
        }
    }
}
