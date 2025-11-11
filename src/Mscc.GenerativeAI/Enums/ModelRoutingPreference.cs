using System.Text.Json.Serialization;

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