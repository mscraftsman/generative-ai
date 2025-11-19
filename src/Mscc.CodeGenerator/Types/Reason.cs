using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<Reason>))]
    public enum Reason
    {
        /// <summary>
        /// A blocked reason was not specified.
        /// </summary>
        BlockedReasonUnspecified,
        /// <summary>
        /// Content was blocked by safety settings.
        /// </summary>
        Safety,
        /// <summary>
        /// Content was blocked, but the reason is uncategorized.
        /// </summary>
        Other,
    }
}