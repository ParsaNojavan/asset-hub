using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.DTOs
{
    public class RenameFileDto
    {
        public Guid FileId { get; set; }
        public string Name { get; set; }
    }
}
