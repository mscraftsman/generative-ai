using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Block at and beyond a specified harm probability.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<HarmBlockThreshold>))]

    public enum HarmBlockThreshold
    {
        /// <summary>
        /// BlockUnspecified means unspecified harm block threshold.
        /// </summary>
        BlockUnspecified = 0,
        /// <summary>
        /// BlockLowAndAbove means block low threshold and above (i.e. block more).
        /// </summary>
        BlockLowAndAbove = 1,
        /// <summary>
        /// BlockMediumAndAbove means block medium threshold and above.
        /// </summary>
        BlockMediumAndAbove = 2,
        /// <summary>
        /// BlockOnlyHigh means block only high threshold (i.e. block less).
        /// </summary>
        BlockOnlyHigh = 3,
        /// <summary>
        /// BlockNone means block none.
        /// </summary>
        BlockNone = 4
    }
}
