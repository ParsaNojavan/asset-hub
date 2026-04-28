using MediatR;
using Sharing.Application.CQRS.Command;
using Sharing.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.CQRS.Handlers
{
    public class DeleteShareCommandHandler : IRequestHandler<DeleteShareCommand>
    {
        private readonly ISharedAssetRepository _sharedAssetRepository;

        public DeleteShareCommandHandler(ISharedAssetRepository sharedAssetRepository)
        {
            _sharedAssetRepository = sharedAssetRepository;
        }

        public async Task Handle(DeleteShareCommand request, CancellationToken cancellationToken)
        {
            var sharedAsset = await _sharedAssetRepository.GetByShareIdAsync(request.shareId);

            if (sharedAsset is null)
                throw new Exception("Not Found!");

            if (sharedAsset.OwnerUserId != request.userId)
                throw new Exception("Forbidden");

            await _sharedAssetRepository.RemoveShareAsync(request.shareId);
        }
    }
}
