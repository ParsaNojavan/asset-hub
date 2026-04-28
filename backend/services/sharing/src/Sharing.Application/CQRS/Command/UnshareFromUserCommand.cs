using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.CQRS.Command
{
    public record UnshareFromUserCommand(Guid shareId, Guid reciverId, Guid userId) : IRequest {}
}
