using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    [JsonConverter(typeof(JsonStringEnumConverter<BlockedReason>))]

    public enum BlockedReason
    {
        /// <summary>
        /// BlockedReasonUnspecified means unspecified blocked reason.
        /// </summary>
        BlockedReasonUnspecified = 0,
        /// <summary>
        /// BlockedReasonSafety means candidates blocked due to safety.
        /// </summary>
        BlockedReasonSafety = 1,
        /// <summary>
        /// BlockedReasonOther means candidates blocked due to other reason.
        /// </summary>
        BlockedReasonOther = 2
    }
}
