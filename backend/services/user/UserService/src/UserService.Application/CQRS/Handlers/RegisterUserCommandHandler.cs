using AuthService.Application.Repositories;
using AuthService.Application.Services.JWT;
using AuthService.Domain.Entities;
using BCrypt.Net;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.CQRS.Command;
using UserService.Application.DTOs.Responses;

namespace UserService.Application.CQRS.Handlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, LoginResultDto>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        public RegisterUserCommandHandler(IUserRepository userRepository
            , IJwtService jwtService, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<LoginResultDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetUserByEmail(request.Email);

            if (existingUser != null)
                throw new Exception("User with this email already exists");

            var user = new User
            {
                Username = request.UserName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                ImgUrl = request.ImgUrl
            };

            await _userRepository.InsertUser(user);

            var refreshTokenValue = _jwtService.GenerateRefreshToken();

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshTokenValue,
                ExpiresAt = DateTime.UtcNow.AddDays(30)
            };

            await _refreshTokenRepository.AddAsync(refreshToken);

            var accessToken = _jwtService.GenerateToken(user);

            return new LoginResultDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                ExpireAt = DateTime.UtcNow.AddMinutes(30)
            };

        }
    }
}
