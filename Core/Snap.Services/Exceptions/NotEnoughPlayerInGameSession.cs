using System;
using System.Runtime.Serialization;

namespace Snap.Services.Exceptions
{
    [Serializable]
    internal class NotEnoughPlayerInGameSession : Exception
    {
        public NotEnoughPlayerInGameSession()
        {
        }

        public NotEnoughPlayerInGameSession(string message) : base(message)
        {
        }

        public NotEnoughPlayerInGameSession(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotEnoughPlayerInGameSession(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}