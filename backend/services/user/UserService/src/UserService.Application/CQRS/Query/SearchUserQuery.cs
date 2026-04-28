using AuthService.Domain.Entities;
using MediatR;
using UserService.Application.DTOs.Responses;

namespace UserService.Application.CQRS.Query
{
    public record SearchUsersQuery(string Query)
    : IRequest<IEnumerable<UserSearchDto>>;
}
