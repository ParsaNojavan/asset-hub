using Assests.Domain.Entities;
using SharedKernel.IntegrationEvents;
using Storage.Application.Messaging;
using Storage.Application.Repositories;
using Storage.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Infrastructure.Services.Storage.Local
{
    public class UserStorageCleanupService : IUserStorageCleanupService
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IEventPublisher _eventPublisher;

        public UserStorageCleanupService(
            IFolderRepository folderRepository,
            IAssetRepository assetRepository,
            IFileStorageService fileStorageService,
            IEventPublisher eventPublisher)
        {
            _folderRepository = folderRepository;
            _assetRepository = assetRepository;
            _fileStorageService = fileStorageService;
            _eventPublisher = eventPublisher;
        }
        public async Task CleanupAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var folders = (await _folderRepository.GetByUserIdAsync(userId)).ToList();
            List<Asset> deletedAssets = new List<Asset>();

            foreach (var folder in folders)
            {
                var assets = (await _assetRepository.GetByFolderIdAsync(folder.Id)).ToList();

                foreach (var asset in assets)
                {
                    await _fileStorageService.DeleteAsync(asset.StoragePath, cancellationToken);
                    await _assetRepository.DeleteAsync(asset.Id);
                    deletedAssets.Add(asset);
                }
                await _folderRepository.DeleteAsync(folder.Id);

            }

            await _fileStorageService.DeleteDirectoryAsync(userId.ToString(), cancellationToken);

            foreach (var asset in deletedAssets)
            {
                await _eventPublisher.PublishAsync(
                    new AssetDeletedEvent
                    {
                        AssetId = asset.Id,
                        DeletedAt = DateTime.UtcNow
                    },
                    cancellationToken);
            }

        }
    }
}
