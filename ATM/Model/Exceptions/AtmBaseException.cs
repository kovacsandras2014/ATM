using System;
using System.Runtime.Serialization;

namespace ATM.Model.Exceptions
{
    [Serializable]
    public class AtmBaseException : Exception
    {
        private const string DefaultMessage = "General ATM machine error!";

        public AtmBaseException()
            : base(DefaultMessage)
        { }

        public AtmBaseException(string message)
            : base(message ?? DefaultMessage)
        { }

        public AtmBaseException(string message, Exception innerException)
            : base(message ?? DefaultMessage, innerException)
        { }

        protected AtmBaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
