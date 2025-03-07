#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif
using System.Runtime.Serialization;

namespace Mscc.GenerativeAI
{
    [JsonConverter(typeof(JsonStringEnumConverter<ImageResponseFormat>))]
    public enum ImageResponseFormat
    {
        [EnumMember(Value = "url")]
        [JsonStringEnumMemberName("url")]
        Url,
        [EnumMember(Value = "b64_json")]
        [JsonStringEnumMemberName("b64_json")]
        B64Json
    }
}