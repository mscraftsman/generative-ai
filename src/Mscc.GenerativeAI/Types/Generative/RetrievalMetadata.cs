namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Metadata related to the retrieval grounding source. This is part of the `GroundingMetadata`
    /// returned when grounding is enabled.
    /// </summary>
    public class RetrievalMetadata
    {
        /// <summary>
        /// Optional. A score indicating how likely it is that a Google Search query could help answer
        /// the prompt. 
        /// </summary>
        /// <remarks>
        /// The score is in the range [0, 1], where 0 is the least likely and 1 is the most likely.
        /// This score is only populated when google search grounding and dynamic retrieval is enabled.
        /// It will be compared to the threshold to determine whether to trigger google search.
        /// </remarks>
        public float? GoogleSearchDynamicRetrievalScore { get; set; }
    }
}