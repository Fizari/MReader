using System;
using System.Collections.Generic;
using System.Text;

namespace MReader.Core.Models
{
    //Does the same job (even simpler job) as System.Windows.Size but when
    //using that, the deserialization is throwing an exception saying 
    //it couldn't find the IsEmpty property of System.Windows.Size.
    //This class is supposed to solve this issue.
    public class ControlSize
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public ControlSize() { }
        public ControlSize(double width, double height)
        {
            Width = width;
            Height = height;
        }
    }
}
