using Assests.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.Repositories
{
    public interface IAssetRepository
    {
        Task InsertAsync(Asset asset);
        Task<IEnumerable<Asset>> GetByFolderIdAsync(Guid folderId);
        Task DeleteByPathPrefixAsync(string path);
        Task<Asset?> GetByIdAsync(Guid id);
        Task<Asset?> GetByPathAsync(string path);
        Task<IEnumerable<Asset>> GetByPathPrefixAsync(string path);

        Task UpdateAsync(Asset asset);
        Task UpdatePathPrefixAsync(string oldPrefix, string newPrefix);

        Task DeleteAsync(Guid id);
    }
}
