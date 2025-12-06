using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The environment being operated.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<ComputerUseEnvironment>))]
    public enum ComputerUseEnvironment
    {
        /// <summary>
        /// 
        /// </summary>
        EnvironmentUnspecified,
        /// <summary>
        /// 
        /// </summary>
        EnvironmentBrowser
    }
}