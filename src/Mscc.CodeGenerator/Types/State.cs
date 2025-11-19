using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<State>))]
    public enum State
    {
        /// <summary>
        /// The default value. This value is used if the state is omitted.
        /// </summary>
        StateUnspecified,
        /// <summary>
        /// `Chunk` is being processed (embedding and vector storage).
        /// </summary>
        StatePendingProcessing,
        /// <summary>
        /// `Chunk` is processed and available for querying.
        /// </summary>
        StateActive,
        /// <summary>
        /// `Chunk` failed processing.
        /// </summary>
        StateFailed,
    }
}