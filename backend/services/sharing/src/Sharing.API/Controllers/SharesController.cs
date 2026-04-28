using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Helpers;
using Sharing.Application.CQRS.Command;
using Sharing.Application.CQRS.Handlers;
using Sharing.Application.CQRS.Query;
using Sharing.Application.DTOs;
using Sharing.Domain.Entities;

namespace Sharing.API.Controllers
{
    [Route("api/shares")]
    [ApiController]
    [Authorize]
    public class SharesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SharesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("check-permission")]
        public async Task<IActionResult> CheckPermission([FromQuery] Guid assetId,
            [FromQuery] Guid userId)
        {

            var query = new CheckPermissionQuery(assetId, userId);
            var hasPermission = await _mediator.Send(query);

            return Ok(new { hasPermission });
        }

        [HttpPost("share/{resourceId:guid}")]
        public async Task<IActionResult> ShareAsset([FromRoute] Guid resourceId)
        {
            var token = HttpContext.Request.Headers["Authorization"]
            .ToString()
            .Replace("Bearer ", "");

            var userId = User.GetUserId();
            var result = await _mediator.Send(
                new CreateShareCommand(resourceId, userId,token)
            );
            return Ok(result);

        }

        [HttpDelete("unshare/{shareId:guid}")]
        public async Task<IActionResult> UnshareAsset([FromRoute] Guid shareId)
        {
            var userId = User.GetUserId();
            await _mediator.Send(
                new DeleteShareCommand(shareId, userId)
            );
            return NoContent();
        }

        [HttpPost("share")]
        public async Task<IActionResult> ShareWithUser(
            [FromBody] ShareRequest shareRequest)
        {
            var userId = User.GetUserId();
            await _mediator.Send(
                new ShareWithUserCommand(shareRequest.ShareId,
                shareRequest.ReciverIds, userId)
            );
            return Ok();
        }

        [HttpDelete("unshare")]
        public async Task<IActionResult> UnshareFromUser([FromBody] UnshareRequest unshareRequest)
        {
            var userId = User.GetUserId();
            await _mediator.Send(
                new UnshareFromUserCommand(unshareRequest.ShareId,
                unshareRequest.ReciverId, userId)
            );
            return NoContent();
        }

        [HttpGet("shared-with-me")]
        public async Task<IActionResult> GetSharedWithMe([FromQuery] int Page = 1,[FromQuery]int PageSize = 10)
        {
            var userId = User.GetUserId();
            var result = await _mediator.Send(new GetSharedWithMeQuery(userId, PageSize, Page));
            return Ok(result);
        }

        [HttpGet("my-shares")]
        public async Task<IActionResult> GetMyShares([FromQuery] int PageSize = 10, [FromQuery] int Page = 1)
        {
            var userId = User.GetUserId();
            var result = await _mediator.Send(new GetMySharesQuery(userId,PageSize,Page));
            return Ok(result);
        }

        [HttpGet("{shareId:guid}")]
        public async Task<IActionResult> GetShareDetails(Guid shareId)
        {
            var userId = User.GetUserId();
            var result = await _mediator.Send(new GetShareDetailsQuery(shareId, userId));
            return Ok(result);
        }



    }
}
