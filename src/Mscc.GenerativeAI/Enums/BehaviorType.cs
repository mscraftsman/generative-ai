using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Specifies the function Behavior.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<BehaviorType>))]
    public enum BehaviorType
    {
        /// <summary>
        /// This value is unused.
        /// </summary>
        Unspecified,
        /// <summary>
        /// If set, the system will wait to receive the function response before continuing the conversation.
        /// </summary>
        Blocking,
        /// <summary>
        /// If set, the system will not wait to receive the function response. Instead, it will attempt to handle function responses as they become available while maintaining the conversation between the user and the model.
        /// </summary>
        Non_Blocking
    }
}