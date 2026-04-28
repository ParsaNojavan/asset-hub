using Assests.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Query
{
    public record GetAssetByIdQuery(Guid resorceId, Guid userId) : IRequest<Asset>{}
}
