using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The tokenization quality used for given media.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<PartMediaResolutionLevel>))]
    public enum PartMediaResolutionLevel
    {
        /// <summary>
        /// Media resolution has not been set.
        /// </summary>
        MediaResolutionUnspecified,
        /// <summary>
        /// Media resolution set to low.
        /// </summary>
        MediaResolutionLow,
        /// <summary>
        /// Media resolution set to medium.
        /// </summary>
        MediaResolutionMedium,
        /// <summary>
        /// Media resolution set to high.
        /// </summary>
        MediaResolutionHigh
    }
}