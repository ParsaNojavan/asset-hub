using Sharing.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.Services
{
    public interface IStorageServiceClient
    {
        Task<AssetDto?> GetAssetAsync(Guid resourceId, string accessToken);
    }
}
