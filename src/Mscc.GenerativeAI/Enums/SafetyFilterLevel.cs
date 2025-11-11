using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Enum that controls the safety filter level for objectionable content.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<SafetyFilterLevel>))]

    public enum SafetyFilterLevel
    {
        /// <summary>
        /// Content with NEGLIGIBLE will be allowed.
        /// </summary>
        BlockLowAndAbove = 0,
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
    }
}
