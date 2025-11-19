using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<Probability>))]
    public enum Probability
    {
        /// <summary>
        /// Probability is unspecified.
        /// </summary>
        HarmProbabilityUnspecified,
        /// <summary>
        /// Content has a negligible chance of being unsafe.
        /// </summary>
        Negligible,
        /// <summary>
        /// Content has a low chance of being unsafe.
        /// </summary>
        Low,
        /// <summary>
        /// Content has a medium chance of being unsafe.
        /// </summary>
        Medium,
        /// <summary>
        /// Content has a high chance of being unsafe.
        /// </summary>
        High,
    }
}