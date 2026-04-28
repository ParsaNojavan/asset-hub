using MediatR;
using Sharing.Application.DTOs;
using Sharing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.CQRS.Query
{
    public record GetMySharesQuery(Guid UserId, int take, int page)
        : IRequest<SharesResponse> { }
}
