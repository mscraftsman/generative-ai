using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<Threshold>))]
    public enum Threshold
    {
        /// <summary>
        /// Threshold is unspecified.
        /// </summary>
        HarmBlockThresholdUnspecified,
        /// <summary>
        /// Content with NEGLIGIBLE will be allowed.
        /// </summary>
        BlockLowAndAbove,
        /// <summary>
        /// Content with NEGLIGIBLE and LOW will be allowed.
        /// </summary>
        BlockMediumAndAbove,
        /// <summary>
        /// Content with NEGLIGIBLE, LOW, and MEDIUM will be allowed.
        /// </summary>
        BlockOnlyHigh,
        /// <summary>
        /// All content will be allowed.
        /// </summary>
        BlockNone,
        /// <summary>
        /// Turn off the safety filter.
        /// </summary>
        Off,
    }
}