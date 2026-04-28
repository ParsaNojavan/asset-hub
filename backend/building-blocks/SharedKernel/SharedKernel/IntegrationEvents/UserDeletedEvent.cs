using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.IntegrationEvents
{
    public record UserDeletedEvent
    {
        public Guid UserId { get; init; }
        public DateTime DeletedAt { get; init; }
    }
}
