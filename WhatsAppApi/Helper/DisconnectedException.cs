using System;

namespace WhatsAppApi.Helper
{
    class ConnectionException : Exception
    {
        public ConnectionException()
        {

        }
        public ConnectionException(string message)
            : base(message)
        {

        }
        public ConnectionException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
