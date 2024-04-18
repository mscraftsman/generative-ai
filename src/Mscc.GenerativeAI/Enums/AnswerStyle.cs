#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Style for grounded answers.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<AnswerStyle>))]
    public enum AnswerStyle
    {
        /// <summary>
        /// Unspecified answer style.
        /// </summary>
        AnswerStyleUnspecified = 0,
        /// <summary>
        /// Succinct but abstract style.
        /// </summary>
        Abstractive = 1,
        /// <summary>
        /// Very brief and extractive style.
        /// </summary>
        Extractive = 2,
        /// <summary>
        /// Verbose style including extra details. The response may be formatted as a sentence, paragraph, multiple paragraphs, or bullet points, etc.
        /// </summary>
        Verbose = 3
    }
}