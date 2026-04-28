using AuthService.Application.Repositories;
using MediatR;
using SharedKernel.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.CQRS.Command;
using UserService.Application.Messaging;

namespace UserService.Application.CQRS.Handlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _tokenRepository;
        private readonly IEventPublisher _eventPublisher;

        public DeleteUserCommandHandler(
            IUserRepository userRepository,
            IEventPublisher eventPublisher,
            IRefreshTokenRepository tokenRepository)
        {
            _userRepository = userRepository;
            _eventPublisher = eventPublisher;
            _tokenRepository = tokenRepository;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserById(request.UserId);

            if (user == null)
                throw new Exception("User not found");

            await _tokenRepository.DeleteByUserId(user.Id);
            await _userRepository.DeleteUser(user.Id);

            await _eventPublisher.PublishAsync(
                new UserDeletedEvent
                {
                    UserId = user.Id,
                    DeletedAt = DateTime.UtcNow
                },
                cancellationToken);
        }
    }

}
