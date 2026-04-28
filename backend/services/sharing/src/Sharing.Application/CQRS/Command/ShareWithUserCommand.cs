using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.CQRS.Command
{
    public record ShareWithUserCommand(Guid shareId, IEnumerable<Guid> reciverIds, Guid userId) : IRequest {}
}
