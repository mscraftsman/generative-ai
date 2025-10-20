#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<StartOfSpeechSensitivity>))]
    public enum StartOfSpeechSensitivity
    {
        /// <summary>
        /// The default is START_SENSITIVITY_HIGH.
        /// </summary>
        StartSensitivityUnspecified,
        /// <summary>
        /// Automatic detection will detect the start of speech more often.
        /// </summary>
        StartSensitivityHigh,
        /// <summary>
        /// Automatic detection will detect the start of speech less often.
        /// </summary>
        StartSensitivityLow
    }
}