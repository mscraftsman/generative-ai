namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Request for image generation.
    /// </summary>
    public class GenerateImagesRequest
    {
        /// <summary>
        /// Required. A text description of the desired image(s). The maximum length is 1000 characters.
        /// </summary>
        public string Prompt { get; set; }
        /// <summary>
        /// Optional. Model to generate the images for.
        /// </summary>
        public string? Model { get; set; }
        /// <summary>
        /// Optional. Number of images to generate.
        /// </summary>
        public int? N { get; set; }
        /// <summary>
        /// Optional. The quality of the image that will be generated. \"hd\" creates images with
        /// finer details and greater consistency across the image.
        /// </summary>
        public string? Quality { get; set; }
        /// <summary>
        /// Optional. The format in which the generated images are returned. Must be one of url or b64_json.
        /// </summary>
        public string? ResponseFormat { get; set; }
        /// <summary>
        /// Optional. The size of the generated images.
        /// </summary>
        public string? Size { get; set; }
        /// <summary>
        /// Optional. The style of the generated images. Must be one of vivid or natural.
        /// </summary>
        public string? Style { get; set; }
        /// <summary>
        /// Optional. A unique identifier representing your end-user.
        /// </summary>
        public string? User { get; set; }
    }
}