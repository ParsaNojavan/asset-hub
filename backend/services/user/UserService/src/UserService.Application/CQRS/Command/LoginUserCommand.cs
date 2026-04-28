using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.DTOs.Responses;

namespace UserService.Application.CQRS.Command
{
    public class LoginUserCommand : IRequest<LoginResultDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginUserCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
