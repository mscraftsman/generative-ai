#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<ApiSpec>))]
    public enum ApiSpec
    {
        ApiSpecUnspecified,
        SimpleSearch,
        ElasticSearch
    }
}