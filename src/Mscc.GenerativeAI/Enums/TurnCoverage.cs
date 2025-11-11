using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Defines which input is included in the user's turn.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<TurnCoverage>))]
    public enum TurnCoverage
    {
        /// <summary>
        /// If unspecified, the default behavior is `TURN_INCLUDES_ONLY_ACTIVITY`.
        /// </summary>
        TurnCoverageUnspecified,
        /// <summary>
        /// The users turn only includes activity since the last turn, excluding inactivity (e.g. silence on the audio stream). This is the default behavior. 
        /// </summary>
        TurnIncludesOnlyActivity,
        /// <summary>
        /// The users turn includes all realtime input since the last turn, including inactivity (e.g. silence on the audio stream). 
        /// </summary>
        TurnIncludesAllInput
    }
}