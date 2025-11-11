using System;

namespace Mscc.GenerativeAI
{
    public class BlockedPromptException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.BlockedPromptException" /> class.
        /// </summary>
        public BlockedPromptException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.BlockedPromptException" /> class 
        /// with a specific message that describes the current exception.
        /// </summary>
        public BlockedPromptException(string? message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.BlockedPromptException" /> class 
        /// with a specific message that describes the current exception and an inner exception.
        /// </summary>
        public BlockedPromptException(string? message, Exception? innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.BlockedPromptException" /> class 
        /// with the block reason message that describes the current exception.
        /// </summary>
        public BlockedPromptException(PromptFeedback promptFeedback) : base(promptFeedback.BlockReasonMessage) { }
    }
}