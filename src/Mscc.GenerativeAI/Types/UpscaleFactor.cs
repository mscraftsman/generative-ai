using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
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