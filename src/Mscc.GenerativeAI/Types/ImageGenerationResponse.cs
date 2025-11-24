using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ImageGenerationResponse
    {
        /// <summary>
        /// Output only. A list of the generated images.
        /// </summary>
        public List<Image> Predictions { get; set; }
    }
}