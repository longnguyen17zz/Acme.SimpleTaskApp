using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Common.Dto
{
    public class FileDto
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileBytes { get; set; }
        public string FileToken { get; set; }

        public FileDto(string fileName, string fileType)
        {
            FileName = fileName;
            FileType = fileType;
        }

        public FileDto() { }
    }
}
