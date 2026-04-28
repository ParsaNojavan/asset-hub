using MediatR;
using SharedKernel.IntegrationEvents;
using Storage.Application.CQRS.Command.Folders.DeleteFolderCommand;
using Storage.Application.Messaging;
using Storage.Application.Repositories;
using Storage.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Handlers.FolderHandlers
{
    public class DeleteFolderCommandHandler : IRequestHandler<DeleteFolderCommand>
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IFileStorageService _storageService;
        private readonly IEventPublisher _eventPublisher;

        public DeleteFolderCommandHandler(IFolderRepository folderRepository, IAssetRepository assetRepository, IFileStorageService storageService, IEventPublisher eventPublisher)
        {
            _folderRepository = folderRepository;
            _assetRepository = assetRepository;
            _storageService = storageService;
            _eventPublisher = eventPublisher;
        }
        public async Task Handle(DeleteFolderCommand request, CancellationToken cancellationToken)
        {
            
            var folder = await _folderRepository.GetByIdAsync(request.FolderId);
            
            if (folder == null)
                throw new Exception("Folder not found");


            if (folder.UserId != request.UserId)
                throw new Exception("Forbidden");

            var prefix = folder.Path;

            var assets = await _assetRepository.GetByPathPrefixAsync(prefix);

            foreach (var asset in assets)
            {
                await _storageService.DeleteAsync(asset.StoragePath, cancellationToken);
            }

            foreach (var asset in assets)
            {
                await _eventPublisher.PublishAsync(
                new AssetDeletedEvent
                {
                    AssetId = asset.Id,
                    DeletedAt = DateTime.UtcNow
                },
                cancellationToken);
            }

            await _assetRepository.DeleteByPathPrefixAsync(prefix);
            await _folderRepository.DeleteSubFoldersAsync(request.FolderId);

            await _folderRepository.DeleteAsync(folder.Id);

            await _storageService.DeleteDirectoryAsync(folder.Path, cancellationToken);

        }
    }
}
