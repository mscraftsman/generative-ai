#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    [JsonConverter(typeof(JsonStringEnumConverter<PersonGeneration>))]
    public enum PersonGeneration
    {
        DontAllow = 0,
        AllowAdult,
        AllowAll
    }
}