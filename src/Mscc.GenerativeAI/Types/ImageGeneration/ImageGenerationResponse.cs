#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageGenerationResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Image> Predictions { get; set; }
    }
}