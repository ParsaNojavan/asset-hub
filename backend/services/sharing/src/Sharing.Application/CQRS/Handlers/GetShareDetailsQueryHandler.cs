using MediatR;
using Sharing.Application.CQRS.Query;
using Sharing.Application.Repositories;
using Sharing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.CQRS.Handlers
{
    public class GetShareDetailsQueryHandler
        : IRequestHandler<GetShareDetailsQuery, SharedAsset>
    {
        private readonly ISharedAssetRepository _shareRepository;

        public GetShareDetailsQueryHandler(ISharedAssetRepository shareRepository)
        {
            _shareRepository = shareRepository;
        }

        public async Task<SharedAsset> Handle(GetShareDetailsQuery request, CancellationToken cancellationToken)
        {
            var sharedAsset = await _shareRepository
                .GetByShareIdAsync(request.ShareId);

            if (sharedAsset is null)
                throw new Exception("Asset not found");

            if (sharedAsset.OwnerUserId != request.UserId)
                throw new Exception("Forbidden");

            return sharedAsset;

        }
    }
}
