using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Command.Folders.RenameFolderCommand
{
    public record RenameFolderCommand(Guid FolderId,Guid UserId, string Name) : IRequest<string>;
}
