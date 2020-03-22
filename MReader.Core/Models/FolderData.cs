using MReader.Core.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace MReader.Core.Models
{
    public class FolderData
    {
        public DirectoryInfo Folder { get; set; }

        public List<ImageData> Files { get; set; }


        public FolderData (DirectoryInfo directory)
        {
            Folder = directory;
            Files = new List<ImageData>();
        }
    }
}
