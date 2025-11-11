using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A list of floats representing an embedding.
    /// Ref: https://ai.google.dev/api/rest/v1beta/ContentEmbedding
    /// </summary>
    public class ContentEmbedding
    {
        /// <summary>
        /// The embedding values.
        /// </summary>
        public List<float> Values { get; set; }
    }
}
