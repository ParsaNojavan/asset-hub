using Sharing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.Repositories
{
    public interface ISharedAssetRepository
    {
        Task<bool> HasShareAsync(Guid assetId, Guid userId);
        Task<Guid> CreateShareAsync(SharedAsset sharedAsset);
        Task RemoveShareAsync(Guid shareId);
        Task RemoveShareByResourceIdAsync(Guid resourceId);
        Task AddUsersAsync(Guid shareId, IEnumerable<Guid> userIds);
        Task RemoveUserAsync(Guid shareId, Guid userId);
        Task<(IEnumerable<SharedAsset> Items, int TotalCount)> GetSharedWithUserAsync(Guid userId, int take, int page);
        Task<(IEnumerable<SharedAsset> Items, int TotalCount)> GetUserSharesAsync(Guid ownerId, int take, int page);
        Task<SharedAsset?> GetByShareIdAsync(Guid shareId);
        Task<bool> AssetIsShared(Guid assetId);


    }
}
