using System;

namespace Mscc.GenerativeAI
{
    public class StopCandidateException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.StopCandidateException" /> class.
        /// </summary>
        public StopCandidateException() { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.StopCandidateException" /> class 
        /// with a specific message that describes the current exception.
        /// </summary>
        public StopCandidateException(string? message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.StopCandidateException" /> class 
        /// with a specific message that describes the current exception and an inner exception.
        /// </summary>
        public StopCandidateException(string? message, Exception? innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.StopCandidateException" /> class 
        /// with the finish message that describes the current exception.
        /// </summary>
        public StopCandidateException(Candidate candidate) : base(candidate.FinishMessage) { }
    }
}