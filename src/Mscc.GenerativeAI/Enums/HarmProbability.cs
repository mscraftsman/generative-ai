#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The probability that a piece of content is harmful.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<HarmProbability>))]
    public enum HarmProbability
    {
        /// <summary>
        /// Unspecified means harm probability unspecified.
        /// </summary>
        HarmProbabilityUnspecified = 0,
        /// <summary>
        /// Negligible means negligible level of harm.
        /// </summary>
        Negligible = 1,
        /// <summary>
        /// Low means low level of harm.
        /// </summary>
        Low = 2,
        /// <summary>
        /// Medium means medium level of harm.
        /// </summary>
        Medium = 3,
        /// <summary>
        /// High means high level of harm.
        /// </summary>
        High = 4
    }
}