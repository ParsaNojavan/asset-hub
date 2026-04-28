using AuthService.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.CQRS.Command;
using UserService.Application.Repositories;
using UserService.Application.Services;

namespace UserService.Application.CQRS.Handlers
{
    public class ResetPasswordCommandHandler
    : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IPasswordResetTokenRepository _tokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public ResetPasswordCommandHandler(
            IPasswordResetTokenRepository tokenRepository,
            IUserRepository userRepository,
            ITokenService tokenService)
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task Handle(
            ResetPasswordCommand request,
            CancellationToken cancellationToken)
        {
            var tokenHash = _tokenService.HashToken(request.Token);

            var resetToken = await _tokenRepository.GetByTokenHashAsync(tokenHash);

            if (resetToken is null)
                throw new UnauthorizedAccessException("Invalid token");

            if (resetToken.Used)
                throw new UnauthorizedAccessException("Token already used");

            if (resetToken.ExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Token expired");

            var user = await _userRepository.GetUserById(resetToken.UserId);

            if (user is null)
                throw new Exception("User not found");

            var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            user.PasswordHash = newPasswordHash;

            await _userRepository.UpdateUser(user);

            await _tokenRepository.MarkAsUsedAsync(resetToken.Id);
        }
    }

}
