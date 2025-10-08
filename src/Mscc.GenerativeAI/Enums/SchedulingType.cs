#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Specifies how the response should be scheduled in the conversation.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<SchedulingType>))]
    public enum SchedulingType
    {
        /// <summary>
        /// This value is unused.
        /// </summary>
        SchedulingUnspecified,
        /// <summary>
        /// Only add the result to the conversation context, do not interrupt or trigger generation.
        /// </summary>
        Silent,
        /// <summary>
        /// Add the result to the conversation context, and prompt to generate output without interrupting ongoing generation.
        /// </summary>
        WhenIdle,
        /// <summary>
        /// Add the result to the conversation context, interrupt ongoing generation and prompt to generate output.
        /// </summary>
        Interrupt
    }
}