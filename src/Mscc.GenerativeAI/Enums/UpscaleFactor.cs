
using System.Text.Json;
#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The factor to upscale the image (x2 or x4).
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<UpscaleFactor>))]
    public enum UpscaleFactor
    {
        /// <summary>
        /// Upscale factor 2
        /// </summary>
        [JsonStringEnumMemberName("x2")]
        X2,
        /// <summary>
        /// Upscale factor 4
        /// </summary>
        [JsonStringEnumMemberName("x4")]
        X4
    }
}