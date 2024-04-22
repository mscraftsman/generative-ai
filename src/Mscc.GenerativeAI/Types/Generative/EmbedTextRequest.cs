using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class EmbedTextRequest
    {
        /// <summary>
        /// Optional. The free-form input text that the model will turn into an embedding.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EmbedTextRequest() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">Optional. The free-form input text that the model will turn into an embedding.</param>
        public EmbedTextRequest(string text) : this()
        {
            Text = text;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BatchEmbedTextRequest
    {
        /// <summary>
        /// Optional. The free-form input texts that the model will turn into an embedding. The current limit is 100 texts, over which an error will be thrown.
        /// </summary>
        public List<string>? Texts { get; set; }
        /// <summary>
        /// Optional. Embed requests for the batch. Only one of texts or requests can be set.
        /// </summary>
        public List<EmbedTextRequest>? Requests { get; set; }
    }
}