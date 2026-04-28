using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.CQRS.Query
{
    public record CheckPermissionQuery(Guid AssetId, Guid UserId) : IRequest<bool>;
}
