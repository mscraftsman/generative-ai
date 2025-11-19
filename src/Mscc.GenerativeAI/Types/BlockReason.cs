using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<BlockReason>))]
    public enum BlockReason
    {
        /// <summary>
        /// Default value. This value is unused.
        /// </summary>
        BlockReasonUnspecified,
        /// <summary>
        /// Input was blocked due to safety reasons. Inspect `safety_ratings` to understand which safety category blocked it.
        /// </summary>
        Safety,
        /// <summary>
        /// Input was blocked due to other reasons.
        /// </summary>
        Other,
    }
}