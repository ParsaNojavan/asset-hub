using Sharing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.DTOs
{
    public class SharesResponse
    {
        public IEnumerable<SharedAsset> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
