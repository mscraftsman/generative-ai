#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
#endif

namespace Mscc.GenerativeAI
{
    public class UploadFileException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.UploadFileException" /> class.
        /// </summary>
        public UploadFileException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.UploadFileException" /> class 
        /// with a specific message that describes the current exception.
        /// </summary>
        public UploadFileException(string? message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.UploadFileException" /> class 
        /// with a specific message that describes the current exception and an inner exception.
        /// </summary>
        public UploadFileException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}