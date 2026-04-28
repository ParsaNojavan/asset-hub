using Dapper;
using Sharing.Application.Repositories;
using Sharing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sharing.Infrastructure.Repositories
{
    public class SharedAssetRepository : ISharedAssetRepository
    {
        private readonly IDbConnection _db;

        public SharedAssetRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task AddUsersAsync(Guid shareId, IEnumerable<Guid> userIds)
        {
            const string sql = @"INSERT INTO 
                                SharedUsers (Id, ShareId, UserId, CreatedAt)
                                VALUES (@Id, @ShareId, @UserId, @CreatedAt)";

            var users = userIds.Select(u => new
            {
                Id = Guid.NewGuid(),
                ShareId = shareId,
                UserId = u,
                CreatedAt = DateTime.UtcNow
            });

            await _db.ExecuteAsync(sql, users);
        }

        public async Task<bool> AssetIsShared(Guid assetId)
        {
            var sql = "SELECT COUNT(1) FROM SharedAssets WHERE ResourceId = @assetId";
            var count = await _db.ExecuteScalarAsync<int>(sql, new { assetId });
            return count > 0;
        }

        public async Task<Guid> CreateShareAsync(SharedAsset sharedAsset)
        {
            sharedAsset.SetCreated();

            const string sql = @"INSERT INTO 
            SharedAssets (Id, OwnerUserId, ResourceId, CreatedAt)
            VALUES (@Id, @OwnerUserId, @ResourceId, @CreatedAt)";

            await _db.ExecuteAsync(sql, new
            {
                Id = sharedAsset.Id,
                OwnerUserId = sharedAsset.OwnerUserId,
                ResourceId = sharedAsset.ResourceId,
                CreatedAt = sharedAsset.CreatedAt,
            });

            return sharedAsset.Id;
        }

        public async Task<SharedAsset?> GetByShareIdAsync(Guid shareId)
        {
            const string sql = @"SELECT SA.*,
                                SU.Id,SU.ShareId,SU.UserId
                                FROM SharedAssets SA
                                LEFT JOIN SharedUsers SU ON SU.ShareId = SA.Id
                                WHERE SA.Id = @ShareId";

            var shareDict = new Dictionary<Guid, SharedAsset>();

            await _db.QueryAsync<SharedAsset, SharedUser, SharedAsset>(
                sql,
                (share, user) =>
                {
                    if (!shareDict.TryGetValue(share.Id, out var existing))
                    {
                        existing = share;
                        existing.Users = new List<SharedUser>();
                        shareDict.Add(existing.Id, existing);
                    }

                    if (user != null)
                        existing.Users.Add(user);

                    return existing;
                },
                new { ShareId = shareId },
                splitOn: "Id"
            );

            return shareDict.Values.FirstOrDefault();
        }

        public async Task<(IEnumerable<SharedAsset> Items, int TotalCount)> GetSharedWithUserAsync(Guid userId, int take, int page)
        {
            var offset = (page - 1) * take;

            const string sql = @"SELECT SA.*
                        FROM SharedAssets SA
                        INNER JOIN SharedUsers SU ON SU.ShareId = SA.Id
                        WHERE SU.UserId = @UserId
                        ORDER BY SA.CreatedAt
                        OFFSET @Offset ROWS FETCH NEXT @Take ROWS ONLY;

                        SELECT COUNT(*)
                        FROM SharedAssets SA
                        INNER JOIN SharedUsers SU ON SU.ShareId = SA.Id
                        WHERE SU.UserId = @UserId;";

            using var multi = await _db.QueryMultipleAsync(sql, new
            {
                UserId = userId,
                Take = take,
                Offset = offset
            });

            var items = await multi.ReadAsync<SharedAsset>();
            var total = await multi.ReadFirstAsync<int>();

            return (items, total);
        }

        public async Task<(IEnumerable<SharedAsset> Items, int TotalCount)> GetUserSharesAsync(Guid ownerId, int take, int page)
        {
            var offset = (page - 1) * take;

            const string sql = @"SELECT * 
                        FROM SharedAssets
                        WHERE OwnerUserId = @OwnerId
                        ORDER BY CreatedAt
                        OFFSET @Offset ROWS FETCH NEXT @Take ROWS ONLY;

                        SELECT COUNT(*)
                        FROM SharedAssets
                        WHERE OwnerUserId = @OwnerId;";

            using var multi = await _db.QueryMultipleAsync(sql, new
            {
                OwnerId = ownerId,
                Take = take,
                Offset = offset
            });

            var items = (await multi.ReadAsync<SharedAsset>()).ToList();
            var total = await multi.ReadFirstAsync<int>();


            return (items, total);
        }


        public async Task<bool> HasShareAsync(Guid assetId, Guid userId)
        {
            const string sql = @"
            SELECT COUNT(1)
            FROM SharedAssets SA
            INNER JOIN SharedUsers SU ON SA.Id = SU.ShareId
            WHERE SA.ResourceId = @AssetId
            AND SU.UserId = @UserId;";

            var count = await _db.ExecuteScalarAsync<int>(sql, new
            {
                AssetId = assetId,
                UserId = userId
            });

            return count > 0;

        }

        public async Task RemoveShareAsync(Guid shareId)
        {
            const string sql = @"DELETE FROM SharedAssets WHERE Id = @ShareId";

            await _db.ExecuteAsync(sql, new { ShareId = shareId });
        }

        public async Task RemoveShareByResourceIdAsync(Guid resourceId)
        {
            const string sql = @"DELETE FROM SharedAssets WHERE ResourceId = @ResourceId";

            await _db.ExecuteAsync(sql, new { ResourceId = resourceId });
        }

        public async Task RemoveUserAsync(Guid shareId, Guid userId)
        {
            const string sql = @"DELETE FROM SharedUsers
                                WHERE ShareId = @ShareId
                                AND UserId = @UserId";

            await _db.ExecuteAsync(sql, new
            {
                ShareId = shareId,
                UserId = userId
            });
        }
    }
}
