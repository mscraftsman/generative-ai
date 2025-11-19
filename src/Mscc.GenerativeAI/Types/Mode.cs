using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<Mode>))]
    public enum Mode
    {
        /// <summary>
        /// Always trigger retrieval.
        /// </summary>
        ModeUnspecified,
        /// <summary>
        /// Run retrieval only when system decides it is necessary.
        /// </summary>
        ModeDynamic,
    }
}