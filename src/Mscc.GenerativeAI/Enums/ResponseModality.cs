#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The requested modalities of the response.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<ResponseModality>))]
    public enum ResponseModality
    {
        /// <summary>
        /// Default value.
        /// </summary>
        ModalityUnspecified = 0,
        /// <summary>
        /// Indicates the model should return text.
        /// </summary>
        Text,
        /// <summary>
        /// Indicates the model should return images.
        /// </summary>
        Image,
        /// <summary>
        /// Indicates the model should return audio.
        /// </summary>
        Audio
    }
}