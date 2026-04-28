using MediatR;
using Sharing.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.CQRS.Query
{
    public record GetUserProfileQuery(Guid UserId) : IRequest<UserProfileDto>{}
}
