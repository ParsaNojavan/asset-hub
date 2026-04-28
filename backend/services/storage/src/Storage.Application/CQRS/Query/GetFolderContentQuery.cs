using Folders.Domain.Entities;
using Assests.Domain.Entities;
using MediatR;
using Storage.Application.DTOs;

namespace Storage.Application.CQRS.Query
{
    public record GetFolderContentQuery(Guid FolderId, Guid UserId) 
        : IRequest<FolderContentDto>{}
}
