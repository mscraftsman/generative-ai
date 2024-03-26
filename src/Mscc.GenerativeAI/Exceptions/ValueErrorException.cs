#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
#endif

namespace Mscc.GenerativeAI
{
    public class ValueErrorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.ValueErrorException" /> class.
        /// </summary>
        public ValueErrorException() { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.ValueErrorException" /> class 
        /// with a specific message that describes the current exception.
        /// </summary>
        public ValueErrorException(string? message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.ValueErrorException" /> class 
        /// with a specific message that describes the current exception and an inner exception.
        /// </summary>
        public ValueErrorException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}