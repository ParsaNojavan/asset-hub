using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.Services
{
    public interface ISharedUserCleanUpService
    {
        Task CleanupAsync(Guid resourceId, CancellationToken cancellationToken = default);
    }
}
