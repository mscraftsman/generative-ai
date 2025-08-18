#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The environment being operated.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<AnswerStyle>))]
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