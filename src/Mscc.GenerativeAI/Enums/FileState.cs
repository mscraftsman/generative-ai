#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// States for the lifecycle of a File.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<FileState>))]
    public enum FileState
    {
        /// <summary>
        /// The default value. This value is used if the state is omitted.
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// File is being processed and cannot be used for inference yet.
        /// </summary>
        Processing = 1,
        /// <summary>
        /// File is processed and available for inference.
        /// </summary>
        Active = 2,
        /// <summary>
        /// File failed processing.
        /// </summary>
        Failed = 3
    }
}