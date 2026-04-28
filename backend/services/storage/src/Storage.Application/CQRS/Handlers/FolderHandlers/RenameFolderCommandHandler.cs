using MediatR;
using Storage.Application.CQRS.Command.Folders.RenameFolderCommand;
using Storage.Application.Repositories;
using Storage.Application.Services;
using Storage.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Handlers.FolderHandlers
{
    public class RenameFolderCommandHandler : IRequestHandler<RenameFolderCommand, string>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly IFileStorageService _fileStorageService;

        public RenameFolderCommandHandler(IFolderRepository folderRepository
            , IFileStorageService fileStorageService
            , IAssetRepository assetRepository)
        {
            _folderRepository = folderRepository;
            _fileStorageService = fileStorageService;
            _assetRepository = assetRepository;
        }

        public async Task<string> Handle(RenameFolderCommand request, CancellationToken cancellationToken)
        {
            var folder = await _folderRepository.GetByIdAsync(request.FolderId);

            if (folder == null)
                throw new Exception("Folder not found");

            if (folder.UserId != request.UserId)
                throw new Exception("Forbidden");

            var oldPath = folder.Path;

            var parentDirectory = Path.GetDirectoryName(oldPath)?.Replace("\\", "/");

            var newPath = $"{parentDirectory}/{request.Name}";

            // rename physical directory
            await _fileStorageService.MoveDirectoryAsync(oldPath, newPath, cancellationToken);

            // update paths in database
            await _assetRepository.UpdatePathPrefixAsync(oldPath + "/", newPath + "/");
            await _folderRepository.UpdatePathPrefixAsync(oldPath + "/", newPath + "/");

            // update current folder
            folder.Name = request.Name;
            folder.Path = newPath;

            await _folderRepository.UpdateAsync(folder);

            return folder.Path;
        }
    }
}
