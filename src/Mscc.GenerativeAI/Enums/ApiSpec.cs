using System.Text.Json.Serialization;

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