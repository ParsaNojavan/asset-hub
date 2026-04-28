using Folders.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.Repositories
{
    public interface IFolderRepository
    {
        Task InsertAsync(Folder folder);
        Task <IEnumerable<Folder>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Folder>> GetSubFoldersAsync(Guid FolderId);
        Task<Folder> GetRootFolder(Guid UserId); 
        Task<Folder?> GetByIdAsync(Guid id);
        Task UpdateAsync(Folder folder);
        Task UpdatePathPrefixAsync(string oldPrefix, string newPrefix);
        Task DeleteSubFoldersAsync(Guid parrentId);
        Task DeleteAsync(Guid id);
    }
}
