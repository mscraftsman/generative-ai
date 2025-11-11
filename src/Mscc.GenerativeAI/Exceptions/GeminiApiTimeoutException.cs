using System;
using System.Net.Http;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Represents errors that occur during Generative AI API calls.
    /// </summary>
    public class GeminiApiTimeoutException : Exception
    {
        /// <summary>
        /// HTTP response from the API.
        /// </summary>
        public HttpResponseMessage? Response { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeminiApiTimeoutException"/> class.
        /// </summary>
        public GeminiApiTimeoutException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeminiApiTimeoutException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public GeminiApiTimeoutException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeminiApiTimeoutException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public GeminiApiTimeoutException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeminiApiTimeoutException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="response">The HTTP response.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public GeminiApiTimeoutException(string message, HttpResponseMessage response, Exception? innerException = null)
            : base(message, innerException)
        {
            Response = response;
        }
    }
}
