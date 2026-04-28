using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Helpers.ErrorHandling
{
    internal class ErrorResponse
    {
        public string Code { get; set; }
        public string Message { get; internal set; }
        public string TraceId { get; internal set; }
    }
}
