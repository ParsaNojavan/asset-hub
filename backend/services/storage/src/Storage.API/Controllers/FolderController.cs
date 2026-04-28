using Folders.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Helpers;
using Storage.Application.CQRS.Command.Folders.DeleteFolderCommand;
using Storage.Application.CQRS.Command.Folders.RenameFolderCommand;
using Storage.Application.CQRS.Command.Folders.UploadFolderCommand;
using Storage.Application.CQRS.Query;
using Storage.Application.DTOs;
using Storage.Application.Repositories;

namespace Storage.API.Controllers
{
    [Route("api/folder")]
    [ApiController]
    [Authorize]
    public class FolderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FolderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateFolder([FromBody] CreateFolderDto dto)
        {
            var userId = User.GetUserId();
            var command = new CreateFolderCommand(userId, dto.Name, dto.ParentFolderId);

            var folderId = await _mediator.Send(command);
            return Ok(new { FolderId = folderId });
        }

        [HttpDelete("delete/{folderId:guid}")]
        public async Task<IActionResult> DeleteFolder([FromRoute]Guid folderId)
        {
            var userId = User.GetUserId(); 

            await _mediator.Send(new DeleteFolderCommand(folderId, userId));

            return NoContent();

        }

        [HttpPut("rename")]
        public async Task<IActionResult> RenameFolder([FromBody]RenameFolderDto renameDto)
        {
            var userId = User.GetUserId();
            var command = new RenameFolderCommand(renameDto.FolderId, userId,renameDto.Name);
            var newPath = await _mediator.Send(command);
            return Ok(new { Path = newPath });
        }

        [HttpGet("{folderId}/contents")]
        public async Task<IActionResult> GetFolderContents(Guid folderId)
        {
            var userId = User.GetUserId();
            var query = new GetFolderContentQuery(folderId,userId);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyFolders()
        {
            var userId = User.GetUserId();
            var folderQuery = new GetFolderByUserIdQuery(userId);

            var folder = await _mediator.Send(folderQuery);

            var contentQuery = new GetFolderContentQuery(folder.Id, userId);
            var content = await _mediator.Send(contentQuery);

            return Ok(content);
        }
    }
}
