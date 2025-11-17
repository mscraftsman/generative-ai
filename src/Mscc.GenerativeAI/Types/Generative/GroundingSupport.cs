using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A collection of supporting references for a segment of the model's response.
    /// </summary>
    public class GroundingSupport
    {
        /// <summary>
        /// The content segment that this support message applies to.
        /// </summary>
        public Segment? Segment { get; set; }
        /// <summary>
        /// A list of indices into the `grounding_chunks` field of the `GroundingMetadata` message.
        /// These indices specify which grounding chunks support the claim made in the content segment.
        /// </summary>
        /// <remarks>
        /// For example, if this field has the values `[1, 3]`, it means that `grounding_chunks[1]` and
        /// `grounding_chunks[3]` are the sources for the claim in the content segment.
        /// </remarks>
        public List<int>? GroundingChunkIndices { get; set; }
        /// <summary>
        /// The confidence scores for the support references. This list is parallel to the
        /// `grounding_chunk_indices` list. A score is a value between 0.0 and 1.0, with a higher score
        /// indicating a higher confidence that the reference supports the claim. For Gemini 2.0 and
        /// before, this list has the same size as `grounding_chunk_indices`. For Gemini 2.5 and later,
        /// this list is empty and should be ignored.
        /// </summary>
        public List<float>? ConfidenceScores { get; set; }
    }
}