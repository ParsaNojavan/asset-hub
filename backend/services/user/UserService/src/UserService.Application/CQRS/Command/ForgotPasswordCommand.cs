using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.CQRS.Command
{
    public record ForgotPasswordCommand(string Email) : IRequest;

}
