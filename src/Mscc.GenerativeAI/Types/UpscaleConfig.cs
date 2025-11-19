namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class UpscaleConfig
    {
        /// <summary>
        /// Optional. When upscaling, the factor to which the image will be upscaled.
        /// If not specified, the upscale factor will be determined from the longer side of the input image and sampleImageSize.
        /// </summary>
        public UpscaleFactor? UpscaleFactor { get; set; }
    }
}