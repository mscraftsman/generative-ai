#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A list of reasons why content may have been blocked.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<BlockedReason>))]

    public enum BlockedReason
    {
        /// <summary>
        /// BlockedReasonUnspecified means unspecified blocked reason.
        /// </summary>
        BlockedReasonUnspecified = 0,
        /// <summary>
        /// Safety means candidates blocked due to safety.
        /// You can inspect <see cref="SafetyRating"/>s to understand which safety category blocked it.
        /// </summary>
        Safety = 1,
        /// <summary>
        /// Other means candidates blocked due to other reason.
        /// </summary>
        Other = 2
    }
}
