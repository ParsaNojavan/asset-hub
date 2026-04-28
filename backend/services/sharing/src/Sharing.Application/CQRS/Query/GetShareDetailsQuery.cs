using MediatR;
using Sharing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.CQRS.Query
{
    public record GetShareDetailsQuery(Guid ShareId,Guid UserId)
        : IRequest<SharedAsset> {}
}
