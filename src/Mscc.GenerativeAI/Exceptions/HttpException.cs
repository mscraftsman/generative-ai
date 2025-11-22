using System;
using System.Net;

namespace Mscc.GenerativeAI.Exceptions
{
    public class HttpException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public RetryInfo? RetryInfo { get; }

        public HttpException(HttpStatusCode statusCode, string message, RetryInfo? retryInfo = null) : base(message)
        {
            StatusCode = statusCode;
            RetryInfo = retryInfo;
        }
    }
}