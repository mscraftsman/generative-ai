using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The mode of the predictor to be used in dynamic retrieval.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<DynamicRetrievalConfigMode>))]
    public enum DynamicRetrievalConfigMode
    {
        /// <summary>
        /// Always trigger retrieval.
        /// </summary>
        ModeUnspecified = 0,
        /// <summary>
        /// Run retrieval only when system decides it is necessary.
        /// </summary>
        ModeDynamic
    }
}