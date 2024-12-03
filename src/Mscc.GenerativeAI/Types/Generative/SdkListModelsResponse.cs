#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Response for list models.
    /// </summary>
    public class SdkListModelsResponse
    {
        /// <summary>
        /// Output only. A list of the requested embeddings.
        /// </summary>
        public List<SdkModel> Data { get; set; } 
        /// <summary>
        /// Output only. Always "list", required by the SDK.
        /// </summary>
        public string Object { get; set; }
    }
}