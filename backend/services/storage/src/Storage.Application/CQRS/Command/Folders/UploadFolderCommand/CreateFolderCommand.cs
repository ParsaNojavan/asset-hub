using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Command.Folders.UploadFolderCommand
{
    public record CreateFolderCommand(Guid UserId,string Name,Guid ParentFolderId) : IRequest<Guid>;
}
