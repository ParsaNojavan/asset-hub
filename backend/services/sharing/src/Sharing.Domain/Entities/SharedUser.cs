using SharedKernel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Domain.Entities
{
    public class SharedUser : BaseEntity
    {
        public Guid ShareId { get; set; }

        public Guid UserId { get; set; }

        public SharedAsset Share { get; set; } = null!;
    }
}
