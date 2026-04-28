using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Helpers;
using System.Threading;
using UserService.Application.CQRS.Command;
using UserService.Application.CQRS.Query;
using UserService.Application.DTOs.Requests;

namespace UserService.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(userId));
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUser([FromQuery] string query)
        {

            var result = await _mediator.Send(new SearchUsersQuery(query));
            return Ok(result);

        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteMe(CancellationToken cancellationToken)
        {

            var userId = User.GetUserId();
            await _mediator.Send(new DeleteUserCommand { UserId = userId }, cancellationToken);
            return NoContent();

        }

        [HttpGet("me")]
        public async Task<IActionResult> MyProfile()
        {
            var userId = User.GetUserId();
            var user = await _mediator.Send(new GetUserProfileQuery(userId));
            return Ok(user);
        }

        [HttpPatch("update-profile")]
        public async Task<IActionResult> UpdateProfile(
            [FromForm] string? UserName,
            [FromForm] IFormFile? Image,
            CancellationToken cancellationToken)
        {

            var token = HttpContext.Request.Headers["Authorization"]
            .ToString()
            .Replace("Bearer ", "");

            var userId = User.GetUserId();

            byte[]? fileBytes = null;
            string? fileName = null;

            if (Image != null && Image.Length > 0)
            {
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };

                if (!allowedTypes.Contains(Image.ContentType))
                    return BadRequest("Invalid image type");

                if (Image.Length > 2 * 1024 * 1024)
                    return BadRequest("Image size cannot exceed 2MB");

                await using var ms = new MemoryStream();
                await Image.CopyToAsync(ms, cancellationToken);
                fileBytes = ms.ToArray();

                var extension = Path.GetExtension(Image.FileName);
                fileName = $"{userId}{extension}";
            }


            var command = new UpdateUserProfileCommand
            {
                UserId = userId,
                Username = UserName,
                ImageBytes = fileBytes,
                ImageFileName = fileName,
                AccessToken = token
            };


            await _mediator.Send(command, cancellationToken);
            return Ok(new { message = "Profile updated successfully." });

        }



        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userId = User.GetUserId();


            await _mediator.Send(new ChangePasswordCommand(
                userId,
                dto.CurrentPassword,
                dto.NewPassword
            ));

            return NoContent();

        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {

            await _mediator.Send(new ForgotPasswordCommand(request.Email));
            return Ok(new { message = "Password reset email sent." });

        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {


            await _mediator.Send(
                new ResetPasswordCommand(
                    request.Token,
                    request.NewPassword));

            return Ok(new { message = "Password has been reset successfully." });


        }
    }
}
