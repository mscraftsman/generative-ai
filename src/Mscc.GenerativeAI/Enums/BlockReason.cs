#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A list of reasons why content may have been blocked.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<BlockReason>))]
    public enum BlockReason
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
        /// Prompt was blocked due to unknown reasons.
        /// </summary>
        Other = 2,
        /// <summary>
        /// Prompt was blocked due to the terms which are included from the terminology blocklist.
        /// </summary>
        Blocklist,
        /// <summary>
        /// Prompt was blocked due to prohibited content.
        /// </summary>
        ProhibitedContent,
        /// <summary>
        /// Candidates blocked due to unsafe image generation content.
        /// </summary>
        ImageSafety,
        /// <summary>
        /// The prompt was blocked as a jailbreak attempt.
        /// </summary>
        Jailbreak
    }
}
