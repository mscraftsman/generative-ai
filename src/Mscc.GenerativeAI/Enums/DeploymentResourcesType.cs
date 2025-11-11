using System.Text.Json.Serialization;

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