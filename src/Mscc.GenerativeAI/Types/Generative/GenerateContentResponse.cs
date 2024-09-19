#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Response from the model supporting multiple candidates.
    /// Ref: https://ai.google.dev/api/rest/v1beta/GenerateContentResponse
    /// </summary>
    public class GenerateContentResponse
    {
        /// <summary>
        /// A convenience property to get the responded text information of first candidate.
        /// </summary>
        [JsonIgnore]
		public string? Text
        {
            get
            {
                if (Candidates?.FirstOrDefault()?.FinishReason is
                    FinishReason.MaxTokens or
                    FinishReason.Safety or
                    FinishReason.Recitation or
                    FinishReason.Other)
                    return string.Empty;
                return Candidates?.FirstOrDefault()?.Content?.Parts.FirstOrDefault()?.Text;
            }
        }

        /// <summary>
        /// Output only. Generated Candidate responses from the model.
        /// </summary>
        public List<Candidate>? Candidates { get; set; }
        /// <summary>
        /// Output only. Content filter results for a prompt sent in the request.
        /// Note: Sent only in the first stream chunk.
        /// Only happens when no candidates were generated due to content violations.
        /// </summary>
        public PromptFeedback? PromptFeedback { get; set; }
        /// <summary>
        /// Usage metadata about the response(s).
        /// </summary>
        public UsageMetadata? UsageMetadata { get; set; }

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