using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Harm severity levels.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<HarmSeverity>))]
    public enum HarmSeverity
    {
        /// <summary>
        /// Unspecified means harm probability unspecified.
        /// </summary>
        HarmSeverityUnspecified = 0,
        /// <summary>
        /// Negligible means negligible level of harm.
        /// </summary>
        HarmSeverityNegligible = 1,
        /// <summary>
        /// Low means low level of harm.
        /// </summary>
        HarmSeverityLow = 2,
        /// <summary>
        /// Medium means medium level of harm.
        /// </summary>
        HarmSeverityMedium = 3,
        /// <summary>
        /// High means high level of harm.
        /// </summary>
        HarmSeverityHigh = 4
    }
}