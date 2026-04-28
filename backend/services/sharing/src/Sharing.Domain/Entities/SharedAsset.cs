using SharedKernel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Domain.Entities
{
    public class SharedAsset : BaseEntity
    {
        public Guid OwnerUserId { get; set; }

        public Guid ResourceId { get; set; }
        public List<SharedUser> Users { get; set; } = new();
    }

}
