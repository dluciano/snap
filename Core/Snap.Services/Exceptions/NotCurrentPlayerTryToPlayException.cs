using System;
using System.Runtime.Serialization;

namespace Snap.Services.Exceptions
{
    [Serializable]
    internal class NotCurrentPlayerTryToPlayException : Exception
    {
        public NotCurrentPlayerTryToPlayException()
        {
        }

        public NotCurrentPlayerTryToPlayException(string message) : base(message)
        {
        }

        public NotCurrentPlayerTryToPlayException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotCurrentPlayerTryToPlayException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}