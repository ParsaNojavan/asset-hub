using Folders.Domain.Entities;
using MediatR;
using Storage.Application.CQRS.Command.Files.RenameFileCommand;
using Storage.Application.Services;
using Storage.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Handlers.FileHandlers
{
    public class RenameFileCommandHandler : IRequestHandler<RenameFileCommand, string>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IFileStorageService _storageService;
        public RenameFileCommandHandler(IAssetRepository assetRepository
            , IFileStorageService storageService)
        {
            _assetRepository = assetRepository;
            _storageService = storageService;
        }

        public async Task<string> Handle(RenameFileCommand request, CancellationToken cancellationToken)
        {
            var file = await _assetRepository.GetByIdAsync(request.FileId);
            var fileName = file.FileName;
            var extension = fileName.Split('.').Last();

            if (file == null)
                throw new Exception("Folder not found");

            if (file.UserId != request.UserId)
                throw new Exception("Forbidden");

            string previosDirectory = file.StoragePath;

            string parentDirectory = Path.GetDirectoryName(file.StoragePath).Replace("\\","/");

            file.FileName = $"{request.Name}.{extension}";
            file.StoragePath = parentDirectory + "/" + $"{request.Name}.{extension}";

            await _storageService.MoveFileAsync(previosDirectory, file.StoragePath, cancellationToken);
            await _assetRepository.UpdateAsync(file);

            return file.StoragePath;

        }
    }
}
