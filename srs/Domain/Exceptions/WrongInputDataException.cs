using System;
using System.Net;

namespace Domain.Exceptions
{
    [Serializable]
    public class WrongInputDataException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public WrongInputDataException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
        public WrongInputDataException(string message) : base(message) { }
    }
}
