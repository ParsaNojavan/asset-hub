using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.CQRS.Command
{
    public record CreateShareCommand(Guid resourceId, Guid userId, string accessToken) : IRequest<Guid>{}
}
