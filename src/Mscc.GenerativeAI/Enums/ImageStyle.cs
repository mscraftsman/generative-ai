#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    [JsonConverter(typeof(JsonStringEnumConverter<ImageStyle>))]
    public enum ImageStyle
    {
        Vivid,
        Natural
    }
}