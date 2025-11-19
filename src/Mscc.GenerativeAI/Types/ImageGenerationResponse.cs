using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageGenerationResponse
    {
        /// <summary>
        /// Output only. A list of the generated images.
        /// </summary>
        public List<Image> Predictions { get; set; }
    }
}