using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// The output images response.
    /// </summary>
    public partial class RecontextImageResponse
    {
        /// <summary>
        /// List of generated images.
        /// </summary>
        public List<GeneratedImage>? GeneratedImages { get; set; }
    }
}