using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Response for list models.
    /// </summary>
    public partial class SdkListModelsResponse
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