using Dapper;
using Folders.Domain.Entities;
using Storage.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Infrastructure.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly IDbConnection _db;

        public FolderRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task DeleteAsync(Guid id)
        {
            var sql = "DELETE FROM Folders WHERE Id = @Id";
            await _db.ExecuteAsync(sql,new {Id = id});
        }

        public async Task<Folder?> GetByIdAsync(Guid id)
        {
            var sql = "SELECT * FROM Folders WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<Folder>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Folder>> GetByUserIdAsync(Guid userId)
        {
            var sql = "SELECT * FROM Folders WHERE UserId = @UserId";
            return await _db.QueryAsync<Folder>(sql,new {UserId = userId});
        }

        public async Task DeleteSubFoldersAsync(Guid parrentId)
        {
            var sql = "DELETE FROM Folders WHERE ParentFolderId = @ParrentId";

            await _db.ExecuteAsync(sql, new
            {
                ParrentId = parrentId
            });
        }

        public async Task InsertAsync(Folder folder)
        {
            var sql = @"INSERT INTO Folders (Id, UserId, Name, ParentFolderId, Path, CreatedAt, UpdatedAt)
                        VALUES (@Id, @UserId, @Name, @ParentFolderId, @Path, @CreatedAt, @UpdatedAt)";
            await _db.ExecuteAsync(sql,folder);
        }

        public async Task UpdateAsync(Folder folder)
        {
            var sql = @"UPDATE Folders SET Name = @Name,
                               ParentFolderId = @ParentFolderId,
                               Path = @Path,
                               UpdatedAt = @UpdatedAt
                               WHERE Id = @Id";
            await _db.ExecuteAsync(sql,folder);
        }

        public async Task UpdatePathPrefixAsync(string oldPrefix, string newPrefix)
        {
            var sql = @"UPDATE Folders
                SET Path = REPLACE(Path, @OldPrefix, @NewPrefix)
                WHERE Path LIKE @OldPrefix + '%';
            ";

            await _db.ExecuteAsync(sql, new
            {
                OldPrefix = oldPrefix,
                NewPrefix = newPrefix
            });
        }

        public async Task<IEnumerable<Folder>> GetSubFoldersAsync(Guid FolderId)
        {
            var sql = "SELECT * FROM Folders WHERE ParentFolderId = @FolderId";
            return await _db.QueryAsync<Folder>(sql, new {FolderId = FolderId});
        }

        public async Task<Folder> GetRootFolder(Guid UserId)
        {
            var sql = @"
                        SELECT TOP 1*
                        FROM Folders
                        WHERE UserId = @UserId
                        AND ParentFolderId IS NULL
                    ";

             return await _db.QueryFirstOrDefaultAsync<Folder>(sql, new { UserId = UserId });

        }
    }
}
