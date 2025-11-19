using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Request for counting tokens.
    /// </summary>
    public class CountTokensRequest
    {
        /// <summary>
        /// ID of the model to use. For a list of models, see Google models
        /// (https://cloud.google.com/vertex-ai/generative-ai/docs/learn/models).
        /// </summary>
        public string? Model { get; set; }
        /// <summary>
        /// Input content
        /// </summary>
        public List<Content>? Contents { get; set; }
        /// <summary>
        /// Configuration for counting tokens.
        /// </summary>
        public CountTokensConfig? Config { get; set; }
    }
}