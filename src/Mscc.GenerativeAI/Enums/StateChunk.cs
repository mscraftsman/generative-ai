using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Output only. Current state of the Chunk.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<StateChunk>))]
    public enum StateChunk
    {
        /// <summary>
        /// The default value. This value is used if the state is omitted.
        /// </summary>
        StateUnspecified = 0,
        /// <summary>
        /// Chunk is being processed (embedding and vector storage).
        /// </summary>
        StatePendingProcessing,
        /// <summary>
        /// Chunk is processed and available for querying.
        /// </summary>
        StateActive,
        /// <summary>
        /// Chunk failed processing.
        /// </summary>
        StateFailed,
    }
}