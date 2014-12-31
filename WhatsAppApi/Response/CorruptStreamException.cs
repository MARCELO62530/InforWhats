using System;

namespace WhatsAppApi.Response
{
    class CorruptStreamException : Exception
    {
        public string EMessage { get; private set; }
        public CorruptStreamException(string pMessage)
        {
            // TODO: Complete member initialization
            EMessage = pMessage;
        }
    }
}
