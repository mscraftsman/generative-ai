using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The state of the tuned model.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<StateGeneratedFile>))]
    public enum StateGeneratedFile
    {
        /// <summary>
        /// The default value. This value is used if the state is omitted.
        /// </summary>
        StateUnspecified = 0,
        /// <summary>
        /// Being generated.
        /// </summary>
        Generating,
        /// <summary>
        /// Generated and is ready for download.
        /// </summary>
        Generated,
        /// <summary>
        /// Failed to generate the GeneratedFile.
        /// </summary>
        Failed,
    }
}