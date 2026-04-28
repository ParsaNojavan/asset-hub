using Folders.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Query
{
    public record GetFolderByUserIdQuery(Guid UserId) : IRequest<Folder>;
}
