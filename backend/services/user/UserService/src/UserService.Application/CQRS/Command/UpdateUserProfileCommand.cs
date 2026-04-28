using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.CQRS.Command
{
    public class UpdateUserProfileCommand : IRequest
    {
        public Guid UserId { get; set; }

        public string? Username { get; set; }

        public byte[]? ImageBytes { get; set; }

        public string? ImageFileName { get; set; }

        public string AccessToken { get; set; }
    }

}
