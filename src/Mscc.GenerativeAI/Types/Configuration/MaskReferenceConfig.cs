#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Configuration for a Mask reference image.
    /// </summary>
    public class MaskReferenceConfig
    {
        /// <summary>
        /// Prompts the model to generate a mask instead of you needing to provide
        /// one (unless MASK_MODE_USER_PROVIDED is used).
        /// </summary>
        public MaskReferenceMode? MaskMode { get; set; }
        /// <summary>
        /// A list of up to 5 class ids to use for semantic segmentation.
        /// Automatically creates an image mask based on specific objects.
        /// </summary>
        public List<int>? SegmentationClasses { get; set; }
        /// <summary>
        /// Dilation percentage of the mask provided.
        /// </summary>
        /// <remarks>
        /// Float between 0 and 1.
        /// </remarks>
        public float? MaskDilation { get; set; }
    }
}