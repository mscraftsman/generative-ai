using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<Modality>))]
    public enum Modality
    {
        /// <summary>
        /// Unspecified modality.
        /// </summary>
        ModalityUnspecified,
        /// <summary>
        /// Plain text.
        /// </summary>
        Text,
        /// <summary>
        /// Image.
        /// </summary>
        Image,
        /// <summary>
        /// Video.
        /// </summary>
        Video,
        /// <summary>
        /// Audio.
        /// </summary>
        Audio,
        /// <summary>
        /// Document, e.g. PDF.
        /// </summary>
        Document,
    }
}