using Sharing.Application.Repositories;
using Sharing.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Infrastructure.Services
{
    public class SharedUserCleanUpService : ISharedUserCleanUpService
    {
        private readonly ISharedAssetRepository _shareRepository;

        public SharedUserCleanUpService(ISharedAssetRepository shareRepository)
        {
            _shareRepository = shareRepository;
        }

        public async Task CleanupAsync(Guid resourceId, CancellationToken cancellationToken = default)
        {
            await _shareRepository.RemoveShareByResourceIdAsync(resourceId);
        }
    }
}
