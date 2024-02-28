using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    [JsonConverter(typeof(JsonStringEnumConverter<HarmProbability>))]
    public enum HarmProbability
    {
        /// <summary>
        /// HarmProbabilityUnspecified means harm probability unspecified.
        /// </summary>
        HarmProbabilityUnspecified = 0,
        /// <summary>
        /// HarmProbabilityNegligible means negligible level of harm.
        /// </summary>
        HarmProbabilityNegligible = 1,
        /// <summary>
        /// HarmProbabilityLow means low level of harm.
        /// </summary>
        HarmProbabilityLow = 2,
        /// <summary>
        /// HarmProbabilityMedium means medium level of harm.
        /// </summary>
        HarmProbabilityMedium = 3,
        /// <summary>
        /// HarmProbabilityHigh means high level of harm.
        /// </summary>
        HarmProbabilityHigh = 4
    }
}