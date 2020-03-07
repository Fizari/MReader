using MReader.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MReader.Core.Services
{
    public interface IFileService
    {
        int PreLoadingIndex { get; set; }
        ImageData CurrentImage { get; }
        event EventHandler CurrentImageLoaded;
        bool LoadFile(string filePath);
        ImageData LoadNextImage();
        ImageData LoadPreviousImage();
    }
}
