using System;
using System.Runtime.Serialization;

namespace GameSharp.Services.Impl.Exceptions
{
    [Serializable]
    public class UnauthorizedCreateException : Exception
    {
        public UnauthorizedCreateException()
        {
        }

        public UnauthorizedCreateException(string message) : base(message)
        {
        }

        public UnauthorizedCreateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnauthorizedCreateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}