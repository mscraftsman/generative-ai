using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
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
        When_Idle,
        /// <summary>
        /// Add the result to the conversation context, interrupt ongoing generation and prompt to generate output.
        /// </summary>
        Interrupt
    }
}