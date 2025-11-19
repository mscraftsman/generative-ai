using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Output only. The severity of harm for this category. This enum is not supported in Gemini API.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<HarmSeverity>))]
    public enum HarmSeverity
    {
        /// <summary>
        /// The harm severity is unspecified.
        /// </summary>
        HarmSeverityUnspecified = 0,
        /// <summary>
        /// The harm severity is negligible.
        /// </summary>
        HarmSeverityNegligible,
        /// <summary>
        /// The harm severity is low.
        /// </summary>
        HarmSeverityLow,
        /// <summary>
        /// The harm severity is medium.
        /// </summary>
        HarmSeverityMedium,
        /// <summary>
        /// The harm severity is high.
        /// </summary>
        HarmSeverityHigh
    }
}