#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Linq;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The response from the model, including candidate completions.
    /// </summary>
    public class GenerateTextResponse
    {
        /// <summary>
        /// Responded text information of first candidate.
        /// </summary>
        public string? Text =>
            // if (SafetyFeedback?.FirstOrDefault()?.Output?.Rating?.Blocked!)
            //     return string.Empty;
            Candidates?.FirstOrDefault()?.Output;

        /// <summary>
        /// Candidate responses from the model.
        /// </summary>
        public List<TextCompletion> Candidates { get; set; }
        /// <summary>
        /// A set of content filtering metadata for the prompt and response text.
        /// This indicates which SafetyCategory(s) blocked a candidate from this response, the lowest HarmProbability that triggered a block, and the HarmThreshold setting for that category. This indicates the smallest change to the SafetySettings that would be necessary to unblock at least 1 response.
        /// </summary>
        public List<ContentFilter>? Filters { get; set; }
        /// <summary>
        /// Returns any safety feedback related to content filtering.
        /// </summary>
        public List<SafetyFeedback>? SafetyFeedback { get; set; }

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