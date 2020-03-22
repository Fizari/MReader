using System;
using System.Collections.Generic;
using System.Text;

namespace MReader.Core.Exceptions
{
    public class FailedToLoadSettingsException : Exception
    {
        public FailedToLoadSettingsException ()
        {
        }

        public FailedToLoadSettingsException(string message)
        : base(message)
        {
        }

        public FailedToLoadSettingsException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
