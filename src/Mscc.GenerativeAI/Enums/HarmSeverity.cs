#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif


namespace Mscc.GenerativeAI
{
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