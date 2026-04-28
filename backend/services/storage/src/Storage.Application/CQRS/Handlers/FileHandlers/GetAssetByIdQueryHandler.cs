using Assests.Domain.Entities;
using MediatR;
using Storage.Application.CQRS.Query;
using Storage.Application.Repositories;
using Storage.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Handlers.FileHandlers
{
    public class GetAssetByIdQueryHandler : IRequestHandler<GetAssetByIdQuery, Asset>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IPermissionApiClient _permissionApiClient;
        public GetAssetByIdQueryHandler(
            IAssetRepository assetRepository,
            IPermissionApiClient permissionApiClient)
        {
            _assetRepository = assetRepository;
            _permissionApiClient = permissionApiClient;
        }
        public async Task<Asset> Handle(GetAssetByIdQuery request, CancellationToken cancellationToken)
        {
            var asset = await _assetRepository.GetByIdAsync(request.resorceId);

            if (asset == null)
                throw new KeyNotFoundException("Asset not found");

            if (asset.UserId != request.userId)
            {
                var hasPermission = await _permissionApiClient
                    .HasPermissionAsync(request.resorceId, request.userId);

                if (!hasPermission)
                    throw new UnauthorizedAccessException("No permission to access this asset");

            }

            return asset;

        }
    }
}
