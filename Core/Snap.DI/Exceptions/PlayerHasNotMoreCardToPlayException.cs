using System;
using System.Runtime.Serialization;

namespace Snap.Services.Impl.Exceptions
{
    [Serializable]
    internal class PlayerHasNotMoreCardToPlayException : Exception
    {
        public PlayerHasNotMoreCardToPlayException()
        {
        }

        public PlayerHasNotMoreCardToPlayException(string message) : base(message)
        {
        }

        public PlayerHasNotMoreCardToPlayException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PlayerHasNotMoreCardToPlayException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}