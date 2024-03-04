using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    [JsonConverter(typeof(JsonStringEnumConverter<HarmBlockThreshold>))]

    public enum HarmBlockThreshold
    {
        /// <summary>
        /// HarmBlockUnspecified means unspecified harm block threshold.
        /// </summary>
        HarmBlockUnspecified = 0,
        /// <summary>
        /// HarmBlockLowAndAbove means block low threshold and above (i.e. block more).
        /// </summary>
        HarmBlockLowAndAbove = 1,
        /// <summary>
        /// HarmBlockMediumAndAbove means block medium threshold and above.
        /// </summary>
        HarmBlockMediumAndAbove = 2,
        /// <summary>
        /// HarmBlockOnlyHigh means block only high threshold (i.e. block less).
        /// </summary>
        HarmBlockOnlyHigh = 3,
        /// <summary>
        /// HarmBlockNone means block none.
        /// </summary>
        HarmBlockNone = 4
    }
}
