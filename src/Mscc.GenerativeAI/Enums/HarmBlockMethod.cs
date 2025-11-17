using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The method for blocking content. If not specified, the default behavior is to use the
    /// probability score. This enum is not supported in Gemini API.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<HarmBlockMethod>))]
    public enum HarmBlockMethod
    {
        /// <summary>
        /// The harm block method is unspecified.
        /// </summary>
        HarnBlockMethodUnspecified = 0,
        /// <summary>
        /// The harm block method uses both probability and severity scores.
        /// </summary>
        Severity,
        /// <summary>
        /// The harm block method uses the probability score.
        /// </summary>
        Probability
    }
}