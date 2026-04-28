using MediatR;
using Sharing.Application.CQRS.Query;
using Sharing.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.CQRS.Handlers
{
    public class CheckPermissionQueryHandler
        : IRequestHandler<CheckPermissionQuery, bool>
    {
        private readonly ISharedAssetRepository _repo;

        public CheckPermissionQueryHandler(ISharedAssetRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(CheckPermissionQuery request, CancellationToken cancellationToken)
        {
            return await _repo.HasShareAsync(request.AssetId, request.UserId);
        }
    }

}
