using System;
using System.Runtime.Serialization;

namespace GameSharp.Services.Impl.Exceptions
{
    [Serializable]
    internal class GameNotAcceptingMorePlayersException : Exception
    {
        public GameNotAcceptingMorePlayersException()
        {
        }

        public GameNotAcceptingMorePlayersException(string message) : base(message)
        {
        }

        public GameNotAcceptingMorePlayersException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GameNotAcceptingMorePlayersException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}