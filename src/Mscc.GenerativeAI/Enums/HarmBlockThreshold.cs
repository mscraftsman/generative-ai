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
        /// Threshold is unspecified.
        /// </summary>
        HarmBlockThresholdUnspecified = 0,
        /// <summary>
        /// Content with NEGLIGIBLE will be allowed.
        /// </summary>
        BlockLowAndAbove = 1,
        /// <summary>
        /// Content with NEGLIGIBLE and LOW will be allowed.
        /// </summary>
        BlockMediumAndAbove = 2,
        /// <summary>
        /// Content with NEGLIGIBLE, LOW, and MEDIUM will be allowed.
        /// </summary>
        BlockOnlyHigh = 3,
        /// <summary>
        /// All content will be allowed.
        /// </summary>
        BlockNone = 4,
        /// <summary>
        /// Turn off the safety filter.
        /// </summary>
        None,
        Off
    }
}
