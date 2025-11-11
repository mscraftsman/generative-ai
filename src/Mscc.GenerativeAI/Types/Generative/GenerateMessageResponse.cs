using System;
using System.Collections.Generic;
using System.Linq;

namespace Mscc.GenerativeAI
{
    public class GenerateMessageResponse
    {
        /// <summary>
        /// Responded text information of first candidate.
        /// </summary>
        public string? Text =>
            // if (SafetyFeedback?.FirstOrDefault()?.Output?.Rating?.Blocked!)
            //     return string.Empty;
            Candidates?.FirstOrDefault()?.Content;

        /// <summary>
        /// Candidate response messages from the model.
        /// </summary>
        public List<Message> Candidates { get; set; }
        /// <summary>
        /// The conversation history used by the model.
        /// </summary>
        public List<Message>? Messages { get; set; }
        /// <summary>
        /// A set of content filtering metadata for the prompt and response text.
        /// This indicates which SafetyCategory(s) blocked a candidate from this response, the lowest HarmProbability that triggered a block, and the HarmThreshold setting for that category. This indicates the smallest change to the SafetySettings that would be necessary to unblock at least 1 response.
        /// </summary>
        public List<ContentFilter>? Filters { get; set; }

        /// <summary>
        /// A convenience overload to easily access the responded text.
        /// </summary>
        /// <returns>The responded text information of first candidate.</returns>
        public override string ToString()
        {
            return Text ?? String.Empty;
        }
    }
}