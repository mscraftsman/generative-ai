using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    public class GenerateContentResponse
    {
        /// <summary>
        /// Responded text information of first candidate.
        /// </summary>
		public string? Text
        {
            get { return Candidates?[0].Content?.Parts?[0].Text; }
        }

        /// <summary>
        /// Output only. Generated candidates.
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
    }
}