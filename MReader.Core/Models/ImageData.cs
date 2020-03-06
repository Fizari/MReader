using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MReader.Core.Models
{
    public class ImageData
    {

        public FileInfo File { get; }
        public BitmapImage BitmapImg { get; set; }

        public ImageData(string filePath)
        {
            File = new FileInfo(filePath);
        }

        public ImageData(FileInfo fileInfo)
        {
            File = fileInfo;
        }
    }
}
