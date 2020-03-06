using System;
using System.Collections.Generic;
using System.Text;

namespace MReader.Core.Exceptions
{
    public class FolderEmptyException : Exception
    {

        public FolderEmptyException()
        {
        }

        public FolderEmptyException(string message)
        : base(message)
        {
            //TODO
        }

        public FolderEmptyException(string message, Exception inner)
        : base(message, inner)
        {
            //TODO 
        }
    }
}
