using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Command.Folders.DeleteFolderCommand
{
    public record DeleteFolderCommand(Guid FolderId, Guid UserId) : IRequest;
}
