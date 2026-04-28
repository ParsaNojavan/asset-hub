using Assests.Domain.Entities;
using Folders.Domain.Entities;
using MediatR;
using Storage.Application.CQRS.Command.Files.UploadFileCommand;
using Storage.Application.DTOs;
using Storage.Application.Services;
using Storage.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Handlers.FileHandlers
{
    public class UploadFileCommandHandler
    : IRequestHandler<UploadFileCommand, UploadFileResultDto>
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IFileStorageService _fileStorageService;

        public UploadFileCommandHandler(
            IFolderRepository folderRepository,
            IAssetRepository assetRepository,
            IFileStorageService fileStorageService)
        {
            _folderRepository = folderRepository;
            _assetRepository = assetRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<UploadFileResultDto> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var path = request.Path ?? string.Empty;

            var existingFoldersList = await _folderRepository.GetByUserIdAsync(userId);
            var existingFolders = existingFoldersList
                .ToDictionary(f => (f.ParentFolderId, f.Name), f => f);

            Folder userRootFolder;
            if (!existingFolders.TryGetValue((null, userId.ToString()), out userRootFolder))
            {
                userRootFolder = new Folder
                {
                    Name = userId.ToString(),
                    UserId = userId,
                    ParentFolderId = null,
                    Path = userId.ToString()
                };
                await _folderRepository.InsertAsync(userRootFolder);
                existingFolders[(null, userId.ToString())] = userRootFolder;
            }

            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            string fileName = request.FileName;
            string[] folderParts = parts;

            if (parts.Length > 0)
            {
                var lastPart = parts[^1];
                if (System.IO.Path.HasExtension(lastPart))
                {
                    fileName = lastPart;
                    folderParts = parts[..^1];
                }
            }

 
            Guid? parentFolderId = userRootFolder.Id;
            Folder parentFolder = userRootFolder;
            Folder? currentFolder = null;

            foreach (var part in folderParts)
            {
                if (!existingFolders.TryGetValue((parentFolderId, part), out currentFolder))
                {
                    currentFolder = new Folder
                    {
                        Name = part,
                        UserId = userId,
                        ParentFolderId = parentFolderId,
                        Path = $"{parentFolder.Path}/{part}"
                    };

                    await _folderRepository.InsertAsync(currentFolder);
                    existingFolders[(parentFolderId, part)] = currentFolder;
                }

                parentFolderId = currentFolder.Id;
                parentFolder = currentFolder;
            }

            currentFolder ??= userRootFolder;


            var existingAssets = await _assetRepository.GetByFolderIdAsync(currentFolder.Id);
            if (existingAssets.Any(a => a.FileName.Equals(fileName, StringComparison.OrdinalIgnoreCase)))
                throw new Exception($"File '{fileName}' already exists in this folder.");


            using var stream = new MemoryStream(request.FileContent);
            var folderPath = Path.Combine(userId.ToString(), path)
                .Replace("\\", "/");
            var savedPath = await _fileStorageService.UploadAsync(
                stream, fileName, request.ContentType, folderPath, cancellationToken);


            var asset = new Asset
            {
                UserId = request.UserId,
                FolderId = currentFolder.Id,
                FileName = fileName,
                OriginalFileName = request.FileName,
                ContentType = request.ContentType,
                Size = request.FileContent.Length,
                StorageProvider = "Local",
                StoragePath = savedPath
            };

            await _assetRepository.InsertAsync(asset);

            return new UploadFileResultDto
            {
                Path = savedPath,
                FileName = fileName,
                Size = request.FileContent.Length
            };
        }
    }
}

