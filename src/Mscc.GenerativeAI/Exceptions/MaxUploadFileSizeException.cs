#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
#endif

namespace Mscc.GenerativeAI
{
    public class MaxUploadFileSizeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.MaxUploadFileSizeException" /> class.
        /// </summary>
        public MaxUploadFileSizeException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.MaxUploadFileSizeException" /> class 
        /// with a specific message that describes the current exception.
        /// </summary>
        public MaxUploadFileSizeException(string? message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.MaxUploadFileSizeException" /> class 
        /// with a specific message that describes the current exception and an inner exception.
        /// </summary>
        public MaxUploadFileSizeException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}