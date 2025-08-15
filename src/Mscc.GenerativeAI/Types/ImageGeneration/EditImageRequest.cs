namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class EditImageRequest : ImageGenerationRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prompt">A text description of the edit to apply to the image.</param>
        public EditImageRequest(string prompt) : base(prompt) { }
    }
}