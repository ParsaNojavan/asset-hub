using Folders.Domain.Entities;
using MediatR;
using Storage.Application.CQRS.Query;
using Storage.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Handlers.FolderHandlers
{
    public class GetFolderByUserIdQueryHandler : IRequestHandler<GetFolderByUserIdQuery, Folder>
    {
        private readonly IFolderRepository _folderRepository;

        public GetFolderByUserIdQueryHandler(IFolderRepository folderRepository)
        {
            _folderRepository = folderRepository;
        }

        public async Task<Folder> Handle(GetFolderByUserIdQuery request, CancellationToken cancellationToken)
        {
            var folders = (await _folderRepository.GetRootFolder(request.UserId));
            return folders;
        }
    }
}
