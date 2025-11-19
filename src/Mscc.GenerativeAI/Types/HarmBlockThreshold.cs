using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The threshold for blocking content. If the harm probability exceeds this threshold, the
    /// content will be blocked.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<HarmBlockThreshold>))]

    public enum HarmBlockThreshold
    {
        /// <summary>
        /// The harm block threshold is unspecified.
        /// </summary>
        HarmBlockThresholdUnspecified = 0,
        /// <summary>
        /// Block content with a low harm probability or higher.
        /// </summary>
        BlockLowAndAbove,
        /// <summary>
        /// Block content with a medium harm probability or higher.
        /// </summary>
        BlockMediumAndAbove,
        /// <summary>
        /// Block content with a high harm probability.
        /// </summary>
        BlockOnlyHigh,
        /// <summary>
        /// Do not block any content, regardless of its harm probability.
        /// </summary>
        BlockNone,
        // /// <summary>
        // /// Do not block any content, regardless of its harm probability.
        // /// </summary>
        // None,
        /// <summary>
        /// Turn off the safety filter entirely.
        /// </summary>
        Off
    }
}
