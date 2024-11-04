#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The state of the tuning job.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<StateTuningJob>))]
    public enum StateTuningJob
    {
        /// <summary>
        /// The default value. This value is unused.
        /// </summary>
        StateUnspecified = 0,
        /// <summary>
        /// The tuning job is running.
        /// </summary>
        JobStateRunning,
        /// <summary>
        /// The tuning job is pending.
        /// </summary>
        JobStatePending,
        /// <summary>
        /// The tuning job failed.
        /// </summary>
        JobStateFailed,
        /// <summary>
        /// The tuning job has been cancelled.
        /// </summary>
        JobStateCancelled
    }
}