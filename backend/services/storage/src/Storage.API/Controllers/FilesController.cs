using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Helpers;
using Storage.Application.CQRS.Command.Files.DeleteFileCommand;
using Storage.Application.CQRS.Command.Files.RenameFileCommand;
using Storage.Application.CQRS.Command.Files.UploadFileCommand;
using Storage.Application.CQRS.Command.Files.UploadUserAvatarCommand;
using Storage.Application.CQRS.Query;
using Storage.Application.DTOs;
using Storage.Application.Repositories;
using Storage.Application.Services;
using System.Threading;

namespace Storage.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IFolderRepository _folderRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IMediator _mediator;


        public FilesController(IFileStorageService fileStorageService, IAssetRepository assetRepository, IFolderRepository folderRepository, IMediator mediator)
        {
            _fileStorageService = fileStorageService;
            _assetRepository = assetRepository;
            _folderRepository = folderRepository;
            _mediator = mediator;
        }

        #region details

        [HttpGet("{resourceId:guid}")]
        public async Task<IActionResult> Deatils([FromRoute] Guid resourceId, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var query = new GetAssetByIdQuery(resourceId, userId);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        #endregion

        #region Upload
        [HttpPost("upload")]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string path, CancellationToken cancellationToken)
        {


            if (file == null || file.Length == 0)
                return BadRequest("File is required");

            byte[] fileBytes;
            await using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms, cancellationToken);
                fileBytes = ms.ToArray();
            }

            var command = new UploadFileCommand
            {
                UserId = User.GetUserId(),
                FileName = file.FileName,
                ContentType = file.ContentType,
                FileContent = fileBytes,
                Path = path
            };

            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        #endregion

        #region Upload Avatar
        [HttpPost("upload-avatar")]
        public async Task<IActionResult> UploadAvatar([FromForm] IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required");

            var userId = User.GetUserId();
            using var stream = file.OpenReadStream();

            var relativePath = await _mediator.Send(
                new UploadUserAvatarCommand(userId, stream, file.ContentType)
            );


            return Ok(new
            {
                url = relativePath
            });
        }
        #endregion

        #region Download File
        [HttpGet("download")]
        public async Task<IActionResult> Download([FromQuery(Name = "FileId")] Guid fileId, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();


            var file = await _mediator.Send(
           new DownloadAssetQuery(fileId, userId)
            );


            return File(
                file.Stream,
                file.ContentType ?? "application/octet-stream",
                file.FileName);

        }
        #endregion

        #region Preview File
        [HttpGet("preview")]
        public async Task<IActionResult> Preview(
            [FromQuery(Name = "FileId")] Guid fileId,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var file = await _mediator.Send(
                new DownloadAssetQuery(fileId, userId),
                cancellationToken
            );

            Response.Headers["Content-Disposition"] = $"inline; filename=\"{file.FileName}\"";

            return File(
                file.Stream,
                file.ContentType ?? "application/octet-stream",
                enableRangeProcessing: true
            );
        }
        #endregion


        #region Delete File
        [HttpDelete("delete/{fileId:guid}")]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid fileId,
            CancellationToken cancellationToken)
        {

            var userId = User.GetUserId();
            var command = new DeleteFileCommand(fileId, userId);

            await _mediator.Send(command);

            return NoContent();
        }
        #endregion

        #region Rename
        [HttpPut("rename")]
        public async Task<IActionResult> Rename([FromBody] RenameFileDto fileDto)
        {
            var userId = User.GetUserId();
            var command = new RenameFileCommand(fileDto.FileId, userId, fileDto.Name);
            var newPath = await _mediator.Send(command);
            return Ok(new { Path = newPath });
        }
        #endregion
    }
}
