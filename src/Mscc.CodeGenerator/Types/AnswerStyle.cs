using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<AnswerStyle>))]
    public enum AnswerStyle
    {
        /// <summary>
        /// Unspecified answer style.
        /// </summary>
        AnswerStyleUnspecified,
        /// <summary>
        /// Succinct but abstract style.
        /// </summary>
        Abstractive,
        /// <summary>
        /// Very brief and extractive style.
        /// </summary>
        Extractive,
        /// <summary>
        /// Verbose style including extra details. The response may be formatted as a sentence, paragraph, multiple paragraphs, or bullet points, etc.
        /// </summary>
        Verbose,
    }
}