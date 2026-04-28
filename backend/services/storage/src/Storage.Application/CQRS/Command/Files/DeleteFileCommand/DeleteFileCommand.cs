using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Command.Files.DeleteFileCommand
{
    public record DeleteFileCommand(Guid FileId, Guid UserId) : IRequest;
}
