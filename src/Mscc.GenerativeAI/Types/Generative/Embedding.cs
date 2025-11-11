using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A list of floats representing an embedding.
    /// Ref: https://ai.google.dev/api/rest/v1beta/Embedding
    /// </summary>
    public class Embedding
    {
        /// <summary>
        /// The embedding values.
        /// </summary>
        public List<float> Value { get; set; }
    }
}
