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
        public ImageAspectRatio? AspectRatio { get; set; }
        /// <summary>
        /// Optional. Specifies the size of generated images. Supported values are `1K`, `2K`, `4K`. If
        /// not specified, the model will use default value `1K`
        /// </summary>
        public ImageSize? ImageSize { get; set; }
        /// <summary>
        /// MIME type of the generated image.
        /// </summary>
        public string? OutputMimeType { get; set; }
        /// <summary>
        /// Compression quality of the generated image (for ``image/jpeg`` only).
        /// </summary>
        public int? OutputCompressionQuality { get; set; }
        /// <summary>
        /// Optional. The image output format for generated images. This field is not supported in
        /// Gemini API.
        /// </summary>
        public ImageOutputOptions? ImageOutputOptions { get; set; }
        /// <summary>
        /// Optional. Controls whether the model can generate people. This field is not supported in
        /// Gemini API.
        /// </summary>
        public PersonGeneration? PersonGeneration { get; set; }
    }
}