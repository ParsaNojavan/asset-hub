using MediatR;

using Sharing.Application.CQRS.Command;
using Sharing.Application.Repositories;
using Sharing.Application.Services;
using Sharing.Domain.Entities;

namespace Sharing.Application.CQRS.Handlers
{
    public class CreateShareCommandHandler : IRequestHandler<CreateShareCommand, Guid>
    {
        private readonly ISharedAssetRepository _shareRepository;
        private readonly IStorageServiceClient _storageServiceClient;

        public CreateShareCommandHandler(
            ISharedAssetRepository shareRepository,
            IStorageServiceClient storageServiceClient)
        {
            _shareRepository = shareRepository;
            _storageServiceClient = storageServiceClient;
        }

        public async Task<Guid> Handle(CreateShareCommand request, CancellationToken cancellationToken)
        {
            var isShared = await _shareRepository.AssetIsShared(request.resourceId);
            if (isShared)
            {
                throw new Exception("Asset already shared");
            }

            var asset = await _storageServiceClient.GetAssetAsync(request.resourceId,request.accessToken);

            if (asset is null)
                throw new Exception("Asset not found");

            if (asset.UserId != request.userId)
                throw new UnauthorizedAccessException();

            var sharedResource = new SharedAsset()
            {
                OwnerUserId = request.userId,
                ResourceId = asset.Id,
            };

            await _shareRepository.CreateShareAsync(sharedResource);
            return request.userId;
        }
    }
}
