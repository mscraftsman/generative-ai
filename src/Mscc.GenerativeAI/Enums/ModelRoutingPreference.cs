#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Mode of function calling to define the execution behavior for function calling.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<ModelRoutingPreference>))]
    public enum ModelRoutingPreference
    {
        Unknown = 0,
        PrioritizeQuality,
        Balanced,
        PrioritizeCost
    }
}