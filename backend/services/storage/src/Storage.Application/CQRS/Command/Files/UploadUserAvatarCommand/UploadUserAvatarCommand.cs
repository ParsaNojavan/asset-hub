using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Command.Files.UploadUserAvatarCommand
{
    public record UploadUserAvatarCommand(Guid UserId,Stream FileStream,string ContentType) : IRequest<string>
    {}
}
