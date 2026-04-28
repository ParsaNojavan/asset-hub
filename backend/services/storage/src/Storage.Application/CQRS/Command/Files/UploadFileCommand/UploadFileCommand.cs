using MediatR;
using Storage.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Command.Files.UploadFileCommand
{
    public class UploadFileCommand : IRequest<UploadFileResultDto>
    {
        public Guid UserId { get; set; }
        public string FileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public byte[] FileContent { get; set; } = null!;
        public string Path { get; set; } = string.Empty;
    }

}
