namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Config for image generation features.
    /// </summary>
    public class ImageConfig
    {
        /// <summary>
        /// Optional. The aspect ratio of the image to generate.
        /// </summary>
        /// <remarks>
        /// Supported aspect ratios: 1:1, 2:3, 3:2, 3:4, 4:3, 9:16, 16:9, 21:9.
        /// If not specified, the model will choose a default aspect ratio based on any reference images provided.
        /// </remarks>
        public ImageAspectRatio AspectRatio { get; set; }
    }
}