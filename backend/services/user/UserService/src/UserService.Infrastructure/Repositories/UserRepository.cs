using AuthService.Application.Repositories;
using AuthService.Domain.Entities;
using Dapper;
using System.Data;

namespace AuthService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _db;

        public UserRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task InsertUser(User user)
        {
            user.SetCreated();
            var sql = @"
                INSERT INTO Users (Id,UserName,Email,PasswordHash,CreatedAt,UpdatedAt,ImgUrl) 
                VALUES (@Id,@UserName,@Email,@PasswordHash,@CreatedAt,@UpdatedAt,@ImgUrl)";
            await _db.ExecuteAsync(sql, user);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var sql = "SELECT * FROM Users WHERE Email = @Email";
            return await _db.QueryFirstOrDefaultAsync<User>(sql, new {Email = email});
        }

        public async Task<User?> GetUserById(Guid id)
        {
            var sql = "SELECT * FROM Users WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<User>(sql, new {Id = id});
        }

        public async Task DeleteUser(Guid userId)
        {
            var sql = "DELETE FROM Users WHERE Id = @Id";
            await _db.ExecuteAsync(sql, new {Id = userId});
        }

        public async Task UpdateUser(User user)
        {
            user.SetUpdated();
            var sql = @"UPDATE Users
                        SET Username = @Username,
                            Email = @Email,
                            PasswordHash = @PasswordHash,
                            ImgUrl = @ImgUrl
                        WHERE Id = @Id";
            await _db.ExecuteAsync(sql, user);
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string query)
        {
            var sql = @"SELECT TOP 10
            Id,Email,UserName,ImgUrl
            FROM Users WHERE
            UserName LIKE '%' + @Query + '%'
            OR Email LIKE '%' + @Query + '%';";

            return await _db.QueryAsync<User>(
                sql,
                new { Query = query}
            );
        }

    }
}
