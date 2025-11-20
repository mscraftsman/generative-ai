namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Request for image editing.
    /// </summary>
    public class EditImageRequest : ImageGenerationRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditImageRequest"/> class.
        /// </summary>
        /// <param name="prompt">A text description of the edit to apply to the image.</param>
        /// <param name="sampleCount">The number of generated images.</param>
        public EditImageRequest(string prompt, int? sampleCount = 4) : base(prompt, sampleCount) { }
    }
}