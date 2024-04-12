#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Probability vs severity.
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
        Severity = 1,
        /// <summary>
        /// The harm block method uses the probability score.
        /// </summary>
        Probability = 2,
    }
}