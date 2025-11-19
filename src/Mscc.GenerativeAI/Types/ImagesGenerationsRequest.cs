namespace Mscc.GenerativeAI
{
    /// <summary>
    /// OpenAI image generation request 
    /// </summary>
    public class ImagesGenerationsRequest
    {
        /// <summary>
        /// A text description of the desired image(s).
        /// </summary>
        public string Prompt { get; set; }

        /// <summary>
        /// Required. The name of the `Model` to use for generating the completion.
        /// The model name will prefixed by \"models/\" if no slash appears in it.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Optional. Amount of candidate completions to generate.
        /// Must be a positive integer. Defaults to 1 if not set.
        /// </summary>
        public int? N { get; set; }

        /// <summary>
        /// The quality of the image that will be generated. hd creates images with finer details and greater consistency across the image. 
        /// </summary>
        public ImageQuality? Quality { get; set; }

        /// <summary>
        /// Optional. The format in which the generated images are returned. Must be one of url or b64_json. URLs are only valid for 60 minutes after the image has been generated.
        /// </summary>
        public ImageResponseFormat? ResponseFormat { get; set; }

        /// <summary>
        /// The size of the generated images.
        /// </summary>
        /// <remarks>
        /// Must be one of 256x256, 512x512, or 1024x1024 for dall-e-2. Must be one of 1024x1024, 1792x1024, or 1024x1792 for dall-e-3 models.
        /// </remarks>
        public ImageSize? Size { get; set; }

        /// <summary>
        /// The style of the generated images. Must be one of vivid or natural. Vivid causes the model to lean towards generating hyper-real and dramatic images. Natural causes the model to produce more natural, less hyper-real looking images.
        /// </summary>
        public ImageStyle? Style { get; set; }

        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// </summary>
        public string? User { get; set; }

        private ImagesGenerationsRequest() { }

        public ImagesGenerationsRequest(string model, string prompt)
        {
            Model = model;
            Prompt = prompt;
        }
    }
}