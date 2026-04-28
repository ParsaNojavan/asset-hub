using Folders.Domain.Entities;
using MediatR;
using Storage.Application.CQRS.Command.Folders.UploadFolderCommand;
using Storage.Application.Repositories;
using Storage.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Handlers.FolderHandlers
{
    public class CreateFolderCommandHandler : IRequestHandler<CreateFolderCommand, Guid>
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IFileStorageService _storageService;

        public CreateFolderCommandHandler(IFolderRepository folderRepository, IFileStorageService storageService)
        {
            _folderRepository = folderRepository;
            _storageService = storageService;
        }

        public async Task<Guid> Handle(CreateFolderCommand request, CancellationToken cancellationToken)
        {
            var parrentFolder = await _folderRepository.GetByIdAsync(request.ParentFolderId);

            var path = request.ParentFolderId != null
                ? $"{parrentFolder.Path}/{request.Name}" :
                $"/{request.Name}";

            var folder = new Folder
            {
                UserId = request.UserId,
                Name = request.Name,
                ParentFolderId = request.ParentFolderId,
                Path = path
            };


            await _storageService.CreateDirectoryAsync(path,cancellationToken);
            await _folderRepository.InsertAsync(folder);

            return folder.Id;
        }
    }
}
