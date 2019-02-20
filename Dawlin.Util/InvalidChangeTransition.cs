﻿using System;
using System.Runtime.Serialization;

namespace Dawlin.Util
{
    [Serializable]
    internal class InvalidChangeTransition : Exception
    {
        public InvalidChangeTransition()
        {
        }

        public InvalidChangeTransition(string message) : base(message)
        {
        }

        public InvalidChangeTransition(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidChangeTransition(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}