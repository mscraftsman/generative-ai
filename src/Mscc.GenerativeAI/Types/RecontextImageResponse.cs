using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The output images response.
    /// </summary>
    public class RecontextImageResponse
    {
        /// <summary>
        /// List of generated images.
        /// </summary>
        public List<GeneratedImage>? GeneratedImages { get; set; }
    }
}