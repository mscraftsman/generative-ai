#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The state of the batch.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<BatchState>))]
    public enum BatchState
    {
        /// <summary>
        /// The batch state is unspecified.
        /// </summary>
        BatchStateUnspecified = 0,
        /// <summary>
        /// The service is preparing to run the batch.
        /// </summary>
        BatchStatePending,
        /// <summary>
        /// The batch is in progress.
        /// </summary>
        BatchStateRunning,
        /// <summary>
        /// The batch completed successfully.
        /// </summary>
        BatchStateSucceeded,
        /// <summary>
        /// The batch failed.
        /// </summary>
        BatchStateFailed,
        /// <summary>
        /// The batch has been cancelled.
        /// </summary>
        BatchStateCancelled,
        /// <summary>
        /// The batch has expired.
        /// </summary>
        BatchStateExpired
    }
}