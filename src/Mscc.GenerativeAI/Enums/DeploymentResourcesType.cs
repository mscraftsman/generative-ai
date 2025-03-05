#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<DeploymentResourcesType>))]
    public enum DeploymentResourcesType
    {
        DeploymentResourcesTypeUnspecified = 0,
        AutomaticResources,
        DedicatedResources,
        SharedResources
    }
}