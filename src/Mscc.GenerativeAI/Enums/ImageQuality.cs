#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
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