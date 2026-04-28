using MediatR;
using SharedKernel.IntegrationEvents;
using Storage.Application.CQRS.Command.Files.DeleteFileCommand;
using Storage.Application.CQRS.Command.Folders.DeleteFolderCommand;
using Storage.Application.Messaging;
using Storage.Application.Repositories;
using Storage.Application.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Handlers.FileHandlers
{
    public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IFileStorageService _storageService;
        private readonly IEventPublisher _eventPublisher;

        public DeleteFileCommandHandler(IAssetRepository assetRepository, IFileStorageService storageService, IEventPublisher eventPublisher)
        {
            _assetRepository = assetRepository;
            _storageService = storageService;
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            var file = await _assetRepository.GetByIdAsync(request.FileId);

            if (file == null)
                throw new Exception("File not found");

            if (file.UserId != request.UserId)
                throw new Exception("Forbidden");

            await _storageService.DeleteAsync(file.StoragePath, cancellationToken);

            await _eventPublisher.PublishAsync(
                new AssetDeletedEvent
                {
                    AssetId = file.Id,
                    DeletedAt = DateTime.UtcNow
                },
                cancellationToken);
        }
    }
}
