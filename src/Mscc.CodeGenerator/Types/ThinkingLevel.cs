using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<ThinkingLevel>))]
    public enum ThinkingLevel
    {
        /// <summary>
        /// Default value.
        /// </summary>
        ThinkingLevelUnspecified,
        /// <summary>
        /// Low thinking level.
        /// </summary>
        Low,
        /// <summary>
        /// High thinking level.
        /// </summary>
        High,
    }
}