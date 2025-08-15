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
        X2,
        X4
    }
}