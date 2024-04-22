namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class Instances
    {
        /// <summary>
        /// The text prompt guides what images the model generates. This field is required for both generation and editing.
        /// </summary>
        public string Prompt { get; set; }

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
    }
}