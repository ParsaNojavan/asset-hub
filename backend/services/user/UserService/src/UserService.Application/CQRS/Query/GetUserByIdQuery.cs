using AuthService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.CQRS.Query
{
    public record GetUserByIdQuery(Guid UserId) : IRequest<User> {}
}
