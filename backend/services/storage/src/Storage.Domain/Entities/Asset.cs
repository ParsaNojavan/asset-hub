using Folders.Domain.Entities;
using SharedKernel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assests.Domain.Entities
{
    public class Asset : BaseEntity
    {
        public Guid FolderId { get; set; }
        public Guid UserId { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public long Size { get; set; }

        // Storage abstraction
        public string StorageProvider { get; set; } = null!;
        public string StoragePath { get; set; } = null!;

        public Folder Folder { get; set; } = null!;
    }
}
