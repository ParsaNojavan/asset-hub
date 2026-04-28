using MediatR;
using Sharing.Application.CQRS.Command;
using Sharing.Application.Repositories;
using Sharing.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.CQRS.Handlers
{
    public class ShareWithUserCommandHandler : IRequestHandler<ShareWithUserCommand>
    {
        private readonly ISharedAssetRepository _shareRepository;

        public ShareWithUserCommandHandler(ISharedAssetRepository shareRepository)
        {
            _shareRepository = shareRepository;
        }

        public async Task Handle(ShareWithUserCommand request, CancellationToken cancellationToken)
        {
            var sharedAsset = await _shareRepository.GetByShareIdAsync(request.shareId);

            if (sharedAsset is null)
                throw new Exception("Asset is not shared");

            if (sharedAsset.OwnerUserId != request.userId)
                throw new UnauthorizedAccessException();

            var alreadyExists = sharedAsset.Users
                .Any(u => request.reciverIds.Contains(u.UserId));

            if (alreadyExists)
            {
                throw new Exception("One or more users already have access");
            }


            await _shareRepository.AddUsersAsync(sharedAsset.Id, request.reciverIds);
        }
    }
}
