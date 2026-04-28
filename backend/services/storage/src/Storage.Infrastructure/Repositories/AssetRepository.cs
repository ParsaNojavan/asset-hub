using Assests.Domain.Entities;
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
    public class AssetRepository : IAssetRepository
    {
        private readonly IDbConnection _db;

        public AssetRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task DeleteAsync(Guid id)
        {
            var sql = "DELETE FROM Assets WHERE Id = @Id";
            await _db.ExecuteAsync(sql,new {Id = id});
        }

        public async Task DeleteByPathPrefixAsync(string path)
        {
            var sql = "DELETE FROM Folders WHERE Path LIKE @Prefix";
            await _db.ExecuteAsync(sql, new { Prefix = path + "/%" });
        }

        public async Task<IEnumerable<Asset>> GetByFolderIdAsync(Guid folderId)
        {
            var sql = @"SELECT * FROM Assets WHERE FolderId = @FolderId";
            return await _db.QueryAsync<Asset>(sql, new {FolderId = folderId});
        }

        public async Task<Asset?> GetByIdAsync(Guid id)
        {
            var sql = "SELECT * FROM Assets WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<Asset>(sql, new {Id = id});
        }

        public async Task<Asset?> GetByPathAsync(string path)
        {
            var sql = "SELECT * FROM Assets WHERE StoragePath = @path";
            return await _db.QueryFirstOrDefaultAsync<Asset>(sql, new { path = path });
        }

        public async Task<IEnumerable<Asset>> GetByPathPrefixAsync(string path)
        {

            var sql = "SELECT * FROM Assets WHERE StoragePath LIKE @Prefix";

            return await _db.QueryAsync<Asset>(sql, new
            {
                Prefix = path + "/%"
            });
        }

        public async Task InsertAsync(Asset asset)
        {
            var sql = @"INSERT INTO Assets (Id, UserId, FolderId, FileName, OriginalFileName, ContentType, Size, StorageProvider, StoragePath, CreatedAt, UpdatedAt)
                        VALUES (@Id, @UserId, @FolderId, @FileName, @OriginalFileName, @ContentType, @Size, @StorageProvider, @StoragePath, @CreatedAt, @UpdatedAt)";

            await _db.ExecuteAsync(sql,asset);
        }

        public async Task UpdateAsync(Asset asset)
        {
            var sql = @"UPDATE Assets
                        SET FileName = @FileName,
                            OriginalFileName = @OriginalFileName,
                            ContentType = @ContentType,
                            Size = @Size,
                            StorageProvider = @StorageProvider,
                            StoragePath = @StoragePath,
                            UpdatedAt = @UpdatedAt
                        WHERE Id = @Id";
            await _db.ExecuteAsync(sql,asset);
        }

        public async Task UpdatePathPrefixAsync(string oldPrefix, string newPrefix)
        {
            var sql = @"UPDATE Assets
                SET StoragePath = REPLACE(StoragePath, @OldPrefix, @NewPrefix)
                WHERE StoragePath LIKE @OldPrefix + '%';
            ";

            await _db.ExecuteAsync(sql, new
            {
                OldPrefix = oldPrefix,
                NewPrefix = newPrefix
            });
        }
    }
}
