namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A URI that can be used to flag a place or review for inappropriate content. This is populated
    /// only when the grounding source is Google Maps. This data type is not supported in Gemini API.
    /// </summary>
    public class GroundingMetadataSourceFlaggingUri
    {
        /// <summary>
        /// The URI that can be used to flag the content.
        /// </summary>
        public string? FlagContentUri { get; set; }
        /// <summary>
        /// The ID of the place or review.
        /// </summary>
        public string? SourceId { get; set; }
    }
}