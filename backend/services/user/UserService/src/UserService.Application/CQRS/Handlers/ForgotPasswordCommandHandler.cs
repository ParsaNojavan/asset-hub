using AuthService.Application.Repositories;
using MediatR;
using SharedKernel.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.CQRS.Command;
using UserService.Application.Repositories;
using UserService.Application.Services;
using UserService.Domain.Entities;

namespace UserService.Application.CQRS.Handlers
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetTokenRepository _tokenRepository;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public ForgotPasswordCommandHandler(
            IUserRepository userRepository,
            IPasswordResetTokenRepository tokenRepository,
            ITokenService tokenService,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _tokenService = tokenService;
            _emailService = emailService;
        }
        public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(request.Email);

            if (user is null)
                throw new UnauthorizedAccessException("Invalid email");

            await _tokenRepository.DeleteByUserIdAsync(user.Id);

            var token = _tokenService.GenerateSecureToken();
            var tokenHash = _tokenService.HashToken(token);

            var resetToken = new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                TokenHash = tokenHash,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                Used = false,
                CreatedAt = DateTime.UtcNow
            };

            await _tokenRepository.CreateAsync(resetToken);

            var resetLink =
       $"https://yourapp.com/reset-password?token={token}";

            var emailBody = $@"
        <h3>Reset your password</h3>
        <p>You requested a password reset.</p>
        <p>Click the link below to reset your password:</p>
        <a href=""{resetLink}"">{resetLink}</a>
        <br/><br/>
        <p>This link will expire in 30 minutes.</p>";


            await _emailService.SendAsync(
               to: user.Email,
               subject: "Reset your password",
               body: emailBody
           );
        }
    }
}
