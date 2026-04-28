using AuthService.Application.DTOs.Responses;
using AuthService.Application.Services.JWT;
using AuthService.Domain.Entities;
using AuthService.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.CQRS.Command;

namespace UserService.Application.CQRS.Handlers
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponseDto>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        public RefreshTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository
            , IUserRepository userRepository, IJwtService jwtService)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _jwtService = jwtService;
        }
        public async Task<RefreshTokenResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var existingToken = await _refreshTokenRepository.GetAsync(request.RefreshToken);
            if (existingToken == null || existingToken.IsRevoked || existingToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new Exception("Invalid or expired refresh token");
            }
            var user = await _userRepository.GetUserById(existingToken.UserId);
            if (user == null) throw new Exception("User not found");

            await _refreshTokenRepository.RevokeAsync(existingToken.Token);

            var newAccessToken = _jwtService.GenerateToken(user);
            var newRefreshTokenValue = _jwtService.GenerateRefreshToken();

            var newRefreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshTokenValue,
                ExpiresAt = DateTime.UtcNow.AddDays(30)
            };

            await _refreshTokenRepository.AddAsync(newRefreshToken);

            return new RefreshTokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshTokenValue
            };

        }
    }
}
