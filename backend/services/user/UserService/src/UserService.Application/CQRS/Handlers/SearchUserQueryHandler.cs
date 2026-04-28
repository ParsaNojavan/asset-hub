using AuthService.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.CQRS.Query;
using UserService.Application.DTOs.Responses;

namespace UserService.Application.CQRS.Handlers
{
    public class SearchUserQueryHandler
        : IRequestHandler<SearchUsersQuery, IEnumerable<UserSearchDto>>
    {
        private readonly IUserRepository _userRepository;
        public SearchUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserSearchDto>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.SearchUsersAsync(
                request.Query
            );

            return users.Select(u => new UserSearchDto
            {
                UserId = u.Id,
                UserName = u.Username,
                Email = u.Email,
                ImgUrl = u.ImgUrl
            }).ToList();
        }
    }
}
