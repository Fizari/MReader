using System;
using System.Collections.Generic;
using System.Text;

namespace MReader.Core.Models
{
    public class WindowSize
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public WindowSize()
        {
            Width = 0;
            Height = 0;
        }

        public WindowSize(double width, double height)
        {
            Width = width;
            Height = height;
        }
    }
}
