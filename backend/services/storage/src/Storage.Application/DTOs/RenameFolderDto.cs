using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.DTOs
{
    public class RenameFolderDto
    {
        public string Name { get; set; }
        public Guid FolderId { get; set; }
    }
}
