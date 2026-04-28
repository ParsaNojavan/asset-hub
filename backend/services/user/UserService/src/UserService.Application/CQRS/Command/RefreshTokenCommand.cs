using AuthService.Application.DTOs.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.CQRS.Command
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponseDto>
    {
        public string RefreshToken { get; set; } = null!;
    }
}
