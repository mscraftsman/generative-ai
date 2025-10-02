#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif
using System.Runtime.Serialization;

namespace Mscc.GenerativeAI
{
    [JsonConverter(typeof(JsonStringEnumConverter<ImageAspectRatio>))]
    public enum ImageAspectRatio
    {
        [EnumMember(Value = "1:1")] 
        [JsonStringEnumMemberName("1:1")]
        Ratio1x1,
        [EnumMember(Value = "9:16")] 
        [JsonStringEnumMemberName("9:16")]
        Ratio9x16,
        [EnumMember(Value = "16:9")] 
        [JsonStringEnumMemberName("16:9")]
        Ratio16x9,
        [EnumMember(Value = "4:3")] 
        [JsonStringEnumMemberName("4:3")]
        Ratio4x3,
        [EnumMember(Value = "3:4")] 
        [JsonStringEnumMemberName("3:4")]
        Ratio3x4,
        [EnumMember(Value = "2:3")] 
        [JsonStringEnumMemberName("2:3")]
        Ratio2x3,
        [EnumMember(Value = "3:2")] 
        [JsonStringEnumMemberName("3:2")]
        Ratio3x2,
        [EnumMember(Value = "21:9")] 
        [JsonStringEnumMemberName("21:9")]
        Ratio21x9
    }
}