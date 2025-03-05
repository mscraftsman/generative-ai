#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The state of a RAG file.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<StateRagFile>))]
    public enum StateRagFile
    {
        /// <summary>
        /// The default value. This value is unused.
        /// </summary>
        StateUnspecified = 0, 
        /// <summary>
        /// The model is ready to be used.
        /// </summary>
        Active,
        /// <summary>
        /// 
        /// </summary>
        Error
    }
}