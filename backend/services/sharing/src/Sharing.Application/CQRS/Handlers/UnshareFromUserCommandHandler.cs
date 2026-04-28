using MediatR;
using Sharing.Application.CQRS.Command;
using Sharing.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.CQRS.Handlers
{
    public class UnshareFromUserCommandHandler 
        : IRequestHandler<UnshareFromUserCommand>
    {
        private readonly ISharedAssetRepository _shareRepository;

        public UnshareFromUserCommandHandler(ISharedAssetRepository shareRepository)
        {
            _shareRepository = shareRepository;
        }

        public async Task Handle(UnshareFromUserCommand request, CancellationToken cancellationToken)
        {
            var sharedAsset = await _shareRepository.GetByShareIdAsync(request.shareId);

            if (sharedAsset is null)
                throw new Exception("Share not found");

            if (sharedAsset.OwnerUserId != request.userId)
                throw new UnauthorizedAccessException();

            var userExists = sharedAsset.Users
                .Any(u => u.UserId == request.reciverId);

            if (!userExists)
                throw new Exception("User does not have access to this share");

            await _shareRepository.RemoveUserAsync(request.shareId, request.reciverId);
        }
    }
}
