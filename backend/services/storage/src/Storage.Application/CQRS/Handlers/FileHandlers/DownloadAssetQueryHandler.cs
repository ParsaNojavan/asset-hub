using MediatR;
using Storage.Application.CQRS.Query;
using Storage.Application.DTOs;
using Storage.Application.Repositories;
using Storage.Application.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Handlers.FileHandlers
{
    public class DownloadAssetQueryHandler
        : IRequestHandler<DownloadAssetQuery, DownloadAssetResult>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IPermissionApiClient _permissionApiClient;
        private readonly IFileStorageService _storageService;
        public DownloadAssetQueryHandler(IAssetRepository assetRepository, IPermissionApiClient permissionApiClient, IFileStorageService storageService)
        {
            _assetRepository = assetRepository;
            _permissionApiClient = permissionApiClient;
            _storageService = storageService;
        }
        public async Task<DownloadAssetResult> Handle(DownloadAssetQuery request, CancellationToken cancellationToken)
        {
            var asset = await _assetRepository.GetByIdAsync(request.AssetId);

            if (asset == null)
                throw new KeyNotFoundException("Asset not found");

            if (asset.UserId != request.UserId)
            {
                var hasPermission = await _permissionApiClient
                    .HasPermissionAsync(request.AssetId,request.UserId);

                if (!hasPermission)
                    throw new UnauthorizedAccessException("No permission to access this asset");
            
            }

            var stream = await _storageService
                .DownloadAsync(asset.StoragePath, cancellationToken);

            if (stream == null)
                throw new Exception("Failed to download file from storage");

            return new DownloadAssetResult
            {
                Stream = stream,
                ContentType = asset.ContentType,
                FileName = asset.FileName
            };
        }
    }
}
