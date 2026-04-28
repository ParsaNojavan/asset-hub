using Assests.Domain.Entities;
using SharedKernel.Base;

namespace Folders.Domain.Entities
{
    public class Folder : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public Guid? ParentFolderId { get; set; }
        public Folder? ParentFolder { get; set; }
        public string Path { get; set; } = null!;

        // Assets
        public List<Asset> Files { get; set; } = new();
    }
}
