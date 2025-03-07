#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif
using System.Runtime.Serialization;

namespace Mscc.GenerativeAI
{
    [JsonConverter(typeof(JsonStringEnumConverter<ImageSize>))]
    public enum ImageSize
    {
        [EnumMember(Value = "256x256")]
        [JsonStringEnumMemberName("256x256")]
        Size256X256,
        [EnumMember(Value = "512x512")]
        [JsonStringEnumMemberName("512x512")]
        Size512X512,
        [JsonStringEnumMemberName("1024x1024")]
        [EnumMember(Value = "1024x1024")]
        Size1024X1024,
        [JsonStringEnumMemberName("1792x1024")]
        [EnumMember(Value = "1792x1024")]
        Size1792X1024,
        [EnumMember(Value = "1024x1792")]
        [JsonStringEnumMemberName("1024x1792")]
        Size1024X1792
    }
}