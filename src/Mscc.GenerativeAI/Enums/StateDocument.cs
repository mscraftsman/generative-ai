#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// States for the lifecycle of a File.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<StateDocument>))]
    public enum StateDocument
    {
        /// <summary>
        /// The default value. This value is used if the state is omitted.
        /// </summary>
        StateUnspecified = 0,
        /// <summary>
        /// Some `Chunks` of the `Document` are being processed (embedding and vector storage).
        /// </summary>
        StatePending,
        /// <summary>
        /// All `Chunks` of the `Document` is processed and available for querying.
        /// </summary>
        StateActive,
        /// <summary>
        /// Some `Chunks` of the `Document` failed processing.
        /// </summary>
        StateFailed
    }
}