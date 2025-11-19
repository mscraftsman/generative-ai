using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<Scheduling>))]
    public enum Scheduling
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
        Interrupt,
    }
}