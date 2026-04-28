using Assests.Domain.Entities;
using Folders.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.DTOs
{
    public class FolderContentDto
    {
        public Guid FolderId { get; set; }
        public Guid? ParentFolderId { get; set; }
        public string Path { get; set; }
        public IEnumerable<Folder> SubFolders { get; set; }
        public IEnumerable<Asset> SubFiles { get; set; }
    }
}
