#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Response for the request to edit an image.
    /// </summary>
    public class EditImageResponse
    {
        /// <summary>
        /// List of generated images.
        /// </summary>
        public List<GeneratedImage>? GeneratedImages { get; set; }
    }
}