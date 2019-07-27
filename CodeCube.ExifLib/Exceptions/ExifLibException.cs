using System;

namespace CodeCube.ExifLib.Exceptions
{
    public class ExifLibException : Exception
    {
        public ExifLibException()
        {
        }

        public ExifLibException(string message)
                : base(message)
        {
        }

        public ExifLibException(string message, Exception innerException)
                : base(message, innerException)
        {
        }
    }
}