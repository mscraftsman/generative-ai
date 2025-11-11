using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Determines how likely detected speech is ended.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<EndOfSpeechSensitivity>))]
    public enum EndOfSpeechSensitivity
    {
        /// <summary>
        /// The default is END_SENSITIVITY_HIGH.
        /// </summary>
        EndSensitivityUnspecified,
        /// <summary>
        /// Automatic detection ends speech more often.
        /// </summary>
        EndSensitivityHigh,
        /// <summary>
        /// Automatic detection ends speech less often.
        /// </summary>
        EndSensitivityLow
    }
}