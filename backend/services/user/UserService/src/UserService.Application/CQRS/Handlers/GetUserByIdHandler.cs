using AuthService.Application.Repositories;
using AuthService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.CQRS.Query;

namespace UserService.Application.CQRS.Handlers
{
    public class GetUserByIdHandler 
        : IRequestHandler<GetUserByIdQuery, User>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserById(request.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not found");
            return user;
        }
    }
}
