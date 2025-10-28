#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

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