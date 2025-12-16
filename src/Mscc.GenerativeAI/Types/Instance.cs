using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// An instance of an image with additional metadata.
    /// </summary>
    public partial class Instance
    {
        /// <summary>
        /// Required. The text prompt for the image.
        /// </summary>
        /// <remarks>
        /// The text prompt guides what images the model generates. This field is required for both generation and editing.
        /// </remarks>
        public string Prompt { get; set; } = string.Empty;

        /// <summary>
        /// Optional. Input image for editing.
        /// </summary>
        /// <remarks>Base64 encoded image (20 MB)</remarks>
        public Image? Image { get; set; }

        /// <summary>
        /// Optional. Mask image for mask-based editing.
        /// </summary>
        /// <remarks>Base64 input image with 1s and 0s where 1 indicates regions to keep (PNG) (20 MB)</remarks>
        public Mask? Mask { get; set; }

        /// <summary>
        /// Optional. A list of reference images for the editing operation.
        /// </summary>
        public List<ReferenceImage> ReferenceImages { get; set; }
    }
}