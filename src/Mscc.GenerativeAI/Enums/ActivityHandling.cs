#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Defines what effect activity has.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<ActivityHandling>))]
    public enum ActivityHandling
    {
        /// <summary>
        /// If unspecified, the default behavior is `START_OF_ACTIVITY_INTERRUPTS`.
        /// </summary>
        ActivityHandlingUnspecified,
        /// <summary>
        /// If true, start of activity will interrupt the model's response (also called \"barge in\"). The model's current response will be cut-off in the moment of the interruption. This is the default behavior.
        /// </summary>
        StartOfActivityInterrupts,
        /// <summary>
        /// The model's response will not be interrupted."
        /// </summary>
        NoInterruption
    }
}