using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Services
{
    public interface IStorageApiClient
    {
        Task<string> UploadProfileImageAsync(
            byte[] imageBytes,
            string fileName,
            string accessToken,
            CancellationToken cancellationToken);
    }
}
