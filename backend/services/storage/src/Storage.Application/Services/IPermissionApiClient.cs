using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.Services
{
    public interface IPermissionApiClient
    {
        Task<bool> HasPermissionAsync(Guid assetId, Guid userId);
    }
}
