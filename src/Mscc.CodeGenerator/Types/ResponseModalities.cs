using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<ResponseModalities>))]
    public enum ResponseModalities
    {
        /// <summary>
        /// Default value.
        /// </summary>
        ModalityUnspecified,
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
        Audio,
    }
}