using MediatR;
using Sharing.Application.CQRS.Query;
using Sharing.Application.DTOs;
using Sharing.Application.Repositories;
using Sharing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.CQRS.Handlers
{
    public class GetSharedWithMeQueryHandler
        : IRequestHandler<GetSharedWithMeQuery, SharesResponse>
    {
        private readonly ISharedAssetRepository _shareRepository;

        public GetSharedWithMeQueryHandler(ISharedAssetRepository shareRepository)
        {
            _shareRepository = shareRepository;
        }

        public async Task<SharesResponse> Handle(GetSharedWithMeQuery request, CancellationToken cancellationToken)
        {
            var (sharedAssets,total) = await _shareRepository
                .GetSharedWithUserAsync(request.UserId,request.take,request.page);

            var response = new SharesResponse
            {
                Items = sharedAssets,
                TotalCount = (int)Math.Ceiling((double)total / request.take)
            };

            return response;
        }
    }
}
