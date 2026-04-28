using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Services.Email
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);
    }
}
