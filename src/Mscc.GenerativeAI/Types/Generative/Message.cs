namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The base unit of structured text.
    /// A Message includes an author and the content of the Message.
    /// The author is used to tag messages when they are fed to the model as text.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Optional. The author of this Message.
        /// This serves as a key for tagging the content of this Message when it is fed to the model as text.
        /// The author can be any alphanumeric string.
        /// </summary>
        public string? Author { get; set; }
        /// <summary>
        /// Required. The text content of the structured Message.
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Output only. Citation information for model-generated content in this Message.
        /// If this Message was generated as output from the model, this field may be populated with attribution
        /// information for any text included in the content. This field is used only on output.
        /// </summary>
        public CitationMetadata? CitationMetadata { get; set; }
    }
}