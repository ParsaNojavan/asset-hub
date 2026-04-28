using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Helpers;
using UserService.Application.CQRS.Command;
using UserService.Application.DTOs;
using UserService.Application.DTOs.Requests;

namespace AuthService.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {

            var tokens = await _mediator.Send(command);
            return Ok(tokens);

        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command)
        {

            var tokens = await _mediator.Send(command);
            return Ok(tokens);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {

            var result = await _mediator.Send(command);
            return Ok(result);

        }

    }
}
