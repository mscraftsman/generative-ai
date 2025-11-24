using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// The quality of the image that will be generated.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<ImageQuality>))]
    public enum ImageQuality
    {
        /// <summary>
        /// Standard quality.
        /// </summary>
        Standard,
        /// <summary>
        /// High definition quality.
        /// </summary>
        Hd
    }
}