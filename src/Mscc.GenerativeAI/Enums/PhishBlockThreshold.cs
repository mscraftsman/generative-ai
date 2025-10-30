#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Optional. Sites with confidence level chosen and above this value will be blocked from the search results.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<PhishBlockThreshold>))]
    public enum PhishBlockThreshold
    {
        /// <summary>
        /// Defaults to unspecified.
        /// </summary>
        PhishBlockThresholdUnspecified = 0,
        /// <summary>
        /// Blocks Low and above confidence URL that is risky.
        /// </summary>
        BlockLowAndAbove,
        /// <summary>
        /// Blocks Medium and above confidence URL that is risky. 
        /// </summary>
        BlockMediumAndAbove,
        /// <summary>
        /// Blocks High and above confidence URL that is risky. 
        /// </summary>
        BlockHighAndAbove,
        /// <summary>
        /// Blocks Higher and above confidence URL that is risky. 
        /// </summary>
        BlockHigherAndAbove,
        /// <summary>
        /// Blocks Very high and above confidence URL that is risky. 
        /// </summary>
        BlockVeryHighAndAbove,
        /// <summary>
        /// Blocks Extremely high confidence URL that is risky.
        /// </summary>
        BlockOnlyExtremelyHigh
    }
}