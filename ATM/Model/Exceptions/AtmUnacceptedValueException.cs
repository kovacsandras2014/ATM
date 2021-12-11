using System;
using System.Runtime.Serialization;

namespace ATM.Model.Exceptions
{
    [Serializable]
    public class AtmUnacceptedValueException : AtmBaseException
    {
        private const string DefaultMessage = "Unaccepted value!";

        public AtmUnacceptedValueException()
            : base(DefaultMessage)
        { }

        public AtmUnacceptedValueException(string message)
            : base(message ?? DefaultMessage)
        { }

        public AtmUnacceptedValueException(string message, Exception innerException)
            : base(message ?? DefaultMessage, innerException)
        { }

        protected AtmUnacceptedValueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

    }
}
