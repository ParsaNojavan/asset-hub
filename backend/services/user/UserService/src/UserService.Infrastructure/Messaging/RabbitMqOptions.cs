using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Infrastructure.Messaging
{
    public class RabbitMqOptions
    {
        public string Host { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Exchange { get; set; } = default!;
    }

}
