using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UserService.Application.DTOs.Responses;

namespace UserService.Application.CQRS.Command
{
    public class RegisterUserCommand : IRequest<LoginResultDto>
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        [JsonIgnore]
        public string ImgUrl { get; set; } = "b4552e8f-831e-467b-8c4c-564c08ab6dbc/profile-pictures/f183aa00-380e-4028-9153-02cba34a37e2.jpg";
    }
}
