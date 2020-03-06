using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using MReader.Core.Exceptions;
using MReader.Core.Extensions;
using MReader.Core.Models;

namespace MReader.Core.Services
{
    public class FileService : IFileService
    {
        private FolderData _folderData;

        //From https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.image?view=netframework-4.8
        private static string[] VALID_EXTENSIONS = { ".bmp", ".gif", ".jpeg", ".jpg", ".png", "wdp", ".tiff" };

        private int _cpt = 0;
        private int _minListSizeForPreLoading;
        private int _preLoadingIndex; // number of next/prev files to load in background

        public event EventHandler CurrentImageLoaded;

        public FolderData CurrentFolder
        {
            get { return _folderData; }
        }

        public ImageData CurrentImage 
        {
            get
            {
                if (_folderData.Files != null && _cpt > -1 && _cpt < _folderData.Files.Count)
                    return _folderData.Files[_cpt];
                else
                    return null;
            }
        }

        public int PreLoadingIndex //todo user can change in options
        {
            get
            {
                return _preLoadingIndex;
            }
            set
            {
                _preLoadingIndex = value;
                _minListSizeForPreLoading = 1 + 2 * _preLoadingIndex;
            }
        }

        public FileService ()
        {
            PreLoadingIndex = 1;
        }

        public ImageData GetNextImage()
        {
            _cpt = ConvertCPT(_cpt + 1);
            if (_folderData.Files.Count > _minListSizeForPreLoading)//to prevent redundancy
            {
                LoadImageData(_cpt + PreLoadingIndex);
                DisposeImageData(_cpt - (PreLoadingIndex + 1));
            }
            return _folderData.Files[_cpt];
        }

        public ImageData GetPreviousFile()
        {
            _cpt = ConvertCPT(_cpt - 1);
            if (_folderData.Files.Count > _minListSizeForPreLoading)//to prevent redundancy
            {
                LoadImageData(_cpt - PreLoadingIndex);
                DisposeImageData(_cpt + (PreLoadingIndex + 1));
            }
            return _folderData.Files[_cpt];
        }

        public bool LoadFile(string filePath)
        {
            var file = new FileInfo(filePath);
            if (!VALID_EXTENSIONS.Contains(file.Extension))
            {
                return false;
            }

            // new folder to be loaded
            if (_folderData == null || _folderData.Folder == null || _folderData.Folder.FullName != file.Directory.FullName) 
            {
                if (_folderData.Files != null)
                    DisposeCurrentlyLoadedFiles();

                _folderData = new FolderData(file.Directory);

                var innerCpt = 0;
                _folderData.Folder.GetFiles().CustomSort().ForEachFile(f =>
                {
                    if (VALID_EXTENSIONS.Contains(f.Extension))
                    {
                        if (f.FullName == filePath)
                            _cpt = innerCpt;
                        _folderData.Files.Add(new ImageData(f));
                        innerCpt++;
                    }
                });
                if (_folderData.Files.Count == 0)
                    throw new FolderEmptyException("The folder is empty (and if you coded that right you should not be there)"); //TODO
                InitLoadingImages();
            }
            else // folder already loaded
            {
                var newCpt = 0;
                var innerCpt = 0;
                _folderData.Files.ForEach(f =>
                {
                    if (filePath == f.File.FullName)
                    {
                        newCpt = innerCpt;
                    }
                    innerCpt++;
                });

                if (newCpt <= _cpt + PreLoadingIndex && newCpt >= _cpt - PreLoadingIndex)
                {//newCpt is within old range
                    CompleteLoadedFiles(newCpt, _cpt);
                    _cpt = newCpt;
                    Imageloaded(_folderData.Files[newCpt]);
                }
                else
                {//dispose
                    DisposeCurrentlyLoadedFiles();
                    _cpt = newCpt;
                    InitLoadingImages();
                }
            }
            return true;
        }

        //To initiate the files first
        public void InitLoadingImages()
        {
            LoadOrDisposeNeighbors(i => LoadImageData(i));
        }

        //To clear the currently loaded files
        public void DisposeCurrentlyLoadedFiles()
        {
            LoadOrDisposeNeighbors(i => DisposeImageData(i));
        }

        private void LoadOrDisposeNeighbors(Action<int> LoadOrDispose)
        {
            var filesCount = _folderData.Files.Count;
            if (filesCount <= _minListSizeForPreLoading)
            {
                for (int i = 0; i < filesCount; i++)
                {
                    LoadOrDispose(i);
                }
            }
            else
            {
                LoadOrDispose(_cpt);
                for (int i = 1; i <= PreLoadingIndex; i++)
                {
                    LoadOrDispose(_cpt + i);
                    LoadOrDispose(_cpt - i);
                }
            }
        }

        //When the file is already loaded (meaning the file is within the 
        //range of the previous load), finish the loading of the range
        public void CompleteLoadedFiles(int newCpt, int oldCpt)
        {
            if (_folderData.Files.Count <= _minListSizeForPreLoading)
                return;
            var offset = newCpt - oldCpt;
            var factor = Math.Sign(offset);
            for (int i = 1; i <= Math.Abs(offset); i++)
            {
                var loadIndex = oldCpt + factor * (PreLoadingIndex + i);
                var disposeIndex = oldCpt - factor * (PreLoadingIndex - 1 + i);
                LoadImageData(loadIndex);
                DisposeImageData(disposeIndex);
            }
        }

        // Convert i to stay in range of the List _files, simulating a circular list
        private int ConvertCPT(int i)
        {
            var filesCount = _folderData.Files.Count;
            var newCpt = i < 0 ? i + Math.Ceiling((Convert.ToDouble(i * -1) / filesCount)) * filesCount : i % filesCount;
            return Convert.ToInt32(newCpt);
        }

        private void LoadImageData(int index)
        {
            //Logic to load Image async here
            ImageData imgToLoad = _folderData.Files[ConvertCPT(index)];
            if (imgToLoad.BitmapImg == null)
            {
                Task.Run(() =>
                {
                    BitmapImage newBitmapImage = new BitmapImage();
                    newBitmapImage.BeginInit();
                    newBitmapImage.UriSource = new Uri(imgToLoad.File.FullName);

                    // To save significant application memory, set the DecodePixelWidth or  
                    // DecodePixelHeight of the BitmapImage value of the image source to the desired 
                    // height or width of the rendered image. If you don't do this, the application will 
                    // cache the image as though it were rendered as its normal size rather then just 
                    // the size that is displayed.
                    // Note: In order to preserve aspect ratio, set DecodePixelWidth
                    // or DecodePixelHeight but not both.
                    //newBitmapImage.DecodePixelWidth = 200;

                    newBitmapImage.EndInit();
                    newBitmapImage.Freeze(); //necessary

                    imgToLoad.BitmapImg = newBitmapImage;

                    Imageloaded(imgToLoad);
                });
            }
        }

        private void DisposeImageData(int index)
        {
            //Logic to dispose Image, MAY BE USELESS
            ImageData imgToDispose = _folderData.Files[ConvertCPT(index)];
            this.PrintDebug("sender : " + imgToDispose.File.Name);
            if (imgToDispose.BitmapImg != null)
            {
                imgToDispose.BitmapImg = null;
            }
        }

        public void Imageloaded(ImageData sender)
        {
            this.PrintDebug("sender : " + sender.File.Name);
            if (sender == CurrentImage && CurrentImageLoaded != null)
                CurrentImageLoaded(sender, new EventArgs());
        }
    }
}
