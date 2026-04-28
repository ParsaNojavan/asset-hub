using AuthService.Application.Services.JWT;
using AuthService.Domain.Entities;
using AuthService.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.DTOs.Responses;
using UserService.Application.CQRS.Command;

namespace UserService.Application.CQRS.Handlers
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResultDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtService _jwtService;

        public LoginUserCommandHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtService = jwtService;
        }
        async Task<LoginResultDto> IRequestHandler<LoginUserCommand, LoginResultDto>.Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password");

            var accessToken = _jwtService.GenerateToken(user);
            var refreshTokenValue = _jwtService.GenerateRefreshToken();

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshTokenValue,
                ExpiresAt = DateTime.UtcNow.AddDays(30)
            };

            await _refreshTokenRepository.AddAsync(refreshToken);

            return new LoginResultDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                ExpireAt = DateTime.UtcNow.AddMinutes(30) 
            };
        }
    }
}
