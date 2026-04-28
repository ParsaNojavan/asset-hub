using MediatR;
using Storage.Application.CQRS.Command.Files.UploadUserAvatarCommand;
using Storage.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Handlers.FileHandlers
{
    public class UploadUserAvatarCommandHandler : IRequestHandler<UploadUserAvatarCommand, string>
    {
        private readonly IFileStorageService _storage;
        public UploadUserAvatarCommandHandler(IFileStorageService storage) => _storage = storage;

        public async Task<string> Handle(UploadUserAvatarCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;

            var relativePath = $"profiles/{userId}.png";

            await _storage.DeleteAsync(relativePath, cancellationToken);

            await _storage.UploadAsync(request.FileStream, $"{userId}.png", request.ContentType, relativePath, cancellationToken);

            return relativePath;
        }
    }

}
