using AuthService.Application.Repositories;
using MediatR;
using Sharing.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.CQRS.Query;

namespace UserService.Application.CQRS.Handlers
{
    public class GetUserProfileQueryHandler
        : IRequestHandler<GetUserProfileQuery, UserProfileDto>
    {
        private readonly IUserRepository _userRepository;

        public GetUserProfileQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        async Task<UserProfileDto> IRequestHandler<GetUserProfileQuery, UserProfileDto>.Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserById(request.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not founed");

            return new UserProfileDto() { 
                UserId = user.Id,
                UserName = user.Username,
                Email = user.Email,
                ImgUrl = user.ImgUrl
            };
        }
    }
}
