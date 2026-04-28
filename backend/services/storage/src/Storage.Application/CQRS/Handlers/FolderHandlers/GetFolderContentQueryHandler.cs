using MediatR;
using Storage.Application.CQRS.Query;
using Storage.Application.DTOs;
using Storage.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Handlers.FolderHandlers
{
    public class GetFolderContentQueryHandler
        : IRequestHandler<GetFolderContentQuery,FolderContentDto>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IFolderRepository _folderRepository;
        public GetFolderContentQueryHandler(IAssetRepository assetRepository, IFolderRepository folderRepository)
        {
            _assetRepository = assetRepository;
            _folderRepository = folderRepository;
        }

        async Task<FolderContentDto> IRequestHandler<GetFolderContentQuery, FolderContentDto>.Handle(GetFolderContentQuery request, CancellationToken cancellationToken)
        {

            var folder = await _folderRepository.GetByIdAsync(request.FolderId);
            var subFolders = await _folderRepository.GetSubFoldersAsync(request.FolderId);
            var subFiles = await _assetRepository.GetByFolderIdAsync(request.FolderId);

            return new FolderContentDto
            {
                FolderId = folder.Id,
                ParentFolderId = folder.ParentFolderId,
                Path = folder.Path,
                SubFolders = subFolders,
                SubFiles = subFiles
            };
        }
    }
}
