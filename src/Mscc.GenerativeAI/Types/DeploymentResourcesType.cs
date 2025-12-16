using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
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