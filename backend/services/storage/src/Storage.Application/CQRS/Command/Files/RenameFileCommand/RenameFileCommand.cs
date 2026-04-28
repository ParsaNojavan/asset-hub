using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Command.Files.RenameFileCommand
{
    public record RenameFileCommand(Guid FileId, Guid UserId, string Name) : IRequest<string>;
}

