#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The state of the tuned model.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<State>))]
    public enum State
    {
        /// <summary>
        /// The default value. This value is unused.
        /// </summary>
        StateUnspecified = 0, 
        /// <summary>
        /// The model is being created.
        /// </summary>
        Creating = 1,
        /// <summary>
        /// The model is ready to be used.
        /// </summary>
        Active = 2,
        /// <summary>
        /// The model failed to be created.
        /// </summary>
        Failed = 3
    }
}