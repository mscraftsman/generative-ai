using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A set of source input(s) for image recontextualization.
    /// </summary>
    public class RecontextImageSource
    {
        /// <summary>
        ///  A text prompt for guiding the model during image recontextualization. Not supported for
        /// Virtual Try-On.
        /// </summary>
        public string? Prompt { get; set; }
        /// <summary>
        /// Image of the person or subject who will be wearing the product(s).
        /// </summary>
        public Image? PersonImage { get; set; }
        /// <summary>
        /// A list of product images.
        /// </summary>
        public List<ProductImage>? ProductImages { get; set; }
    }
}