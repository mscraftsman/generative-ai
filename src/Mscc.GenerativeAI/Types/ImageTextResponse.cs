namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ImageTextResponse
    {
        /// <summary>
        /// List of text strings representing captions, sorted by confidence.
        /// </summary>
        public string[]? Predictions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? DeployedModelId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Model { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? ModelDisplayName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? ModelVersionId { get; set; }
    }
}