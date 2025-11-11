using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Outcome of the code execution.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<Outcome>))]
    public enum Outcome
    {
        /// <summary>
        /// Unspecified status. This value should not be used.
        /// </summary>
        OutcomeUnspecified = 1,
        /// <summary>
        /// Code execution completed successfully.
        /// </summary>
        OutcomeOk,
        /// <summary>
        /// Code execution finished but with a failure. `stderr` should contain the reason.
        /// </summary>
        OutcomeFailed,
        /// <summary>
        /// Code execution ran for too long, and was cancelled. There may or may not be a partial output present.
        /// </summary>
        OutcomeDeadlineExceeded
    }
}