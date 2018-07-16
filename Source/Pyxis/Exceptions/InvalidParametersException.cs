using System;

namespace Pyxis.Exceptions
{
    internal class InvalidParametersException : Exception
    {
        public InvalidParametersException() { }

        public InvalidParametersException(string message) : base(message) { }
    }
}