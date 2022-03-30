using System;
using System.Runtime.Serialization;

namespace TesteApi.Common.Exceptions
{
    [Serializable]
    public class ServiceException : Exception
    {
        public ServiceException() { }

        public ServiceException(string message) : base(CustomMessageService(message)) { }

        public ServiceException(string message, Exception inner) : base(CustomMessageService(message), inner) { }

        public ServiceException(SerializationInfo info, StreamingContext context) { }

        private static string CustomMessageService(string message)
        {
            return message;
        }
    }
}
