using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Output only. The probability of harm for this category.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<HarmProbability>))]
    public enum HarmProbability
    {
        /// <summary>
        /// The harm probability is unspecified.
        /// </summary>
        HarmProbabilityUnspecified = 0,
        /// <summary>
        /// The harm probability is negligible.
        /// </summary>
        Negligible,
        /// <summary>
        /// The harm probability is low.
        /// </summary>
        Low,
        /// <summary>
        /// The harm probability is medium.
        /// </summary>
        Medium,
        /// <summary>
        /// The harm probability is high.
        /// </summary>
        High
    }
}