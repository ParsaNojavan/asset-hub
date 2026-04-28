using AuthService.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.CQRS.Command;
using UserService.Application.Services;

namespace UserService.Application.CQRS.Handlers
{
    public class UpdateUserProfileHandler
    : IRequestHandler<UpdateUserProfileCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IStorageApiClient _storageApiClient;
        private readonly ILogger<UpdateUserProfileHandler> _logger;

        public UpdateUserProfileHandler(
            IUserRepository userRepository,
            IStorageApiClient storageApiClient,
            ILogger<UpdateUserProfileHandler> logger)
        {
            _userRepository = userRepository;
            _storageApiClient = storageApiClient;
            _logger = logger;
        }

        public async Task Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserById(request.UserId);
            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found", request.UserId);
                throw new Exception("User not found");
            }

            if (!string.IsNullOrWhiteSpace(request.Username))
                user.Username = request.Username;

            if (request.ImageBytes != null && request.ImageFileName != null)
            {
                var path = $"profile-pictures/{Guid.NewGuid()}.jpg";

                try
                {
                    var imageUrl = await _storageApiClient.UploadProfileImageAsync(
                        request.ImageBytes,
                        request.ImageFileName,
                        request.AccessToken,
                        cancellationToken);

                    user.ImgUrl = imageUrl;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Upload failed for user {UserId}", request.UserId);
                    throw new Exception("Image upload failed");
                }
            }

            await _userRepository.UpdateUser(user);
        }
    }

}
