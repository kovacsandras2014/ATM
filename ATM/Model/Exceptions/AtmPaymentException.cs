using System;
using System.Runtime.Serialization;

namespace ATM.Model.Exceptions
{
    [Serializable]
    public class AtmPaymentException : AtmBaseException
    {
        private const string DefaultMessage = "General payment error!";

        public AtmPaymentException()
            : base(DefaultMessage)
        { }

        public AtmPaymentException(string message)
            : base(message ?? DefaultMessage)
        { }

        public AtmPaymentException(string message, Exception innerException)
            : base(message ?? DefaultMessage, innerException)
        { }

        protected AtmPaymentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

    }
}
