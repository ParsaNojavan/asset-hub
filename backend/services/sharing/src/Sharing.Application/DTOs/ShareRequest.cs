using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.DTOs
{
    public class ShareRequest
    {
        public Guid ShareId { get; set; }
        public IEnumerable<Guid> ReciverIds { get; set; } = Enumerable.Empty<Guid>();
    }
}
