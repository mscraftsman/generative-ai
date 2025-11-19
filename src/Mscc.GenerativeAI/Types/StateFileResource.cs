using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// States for the lifecycle of a File.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<StateFileResource>))]
    public enum StateFileResource
    {
        /// <summary>
        /// The default value. This value is used if the state is omitted.
        /// </summary>
        StateUnspecified = 0,
        /// <summary>
        /// File is being processed and cannot be used for inference yet.
        /// </summary>
        Processing,
        /// <summary>
        /// File is processed and available for inference.
        /// </summary>
        Active,
        /// <summary>
        /// File failed processing.
        /// </summary>
        Failed
    }
}