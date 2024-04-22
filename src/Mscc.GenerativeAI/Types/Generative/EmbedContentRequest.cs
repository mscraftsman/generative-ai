#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif
using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Request containing the <see cref="Content"/> for the model to embed.
    /// </summary>
    public class EmbedContentRequest
    {
        /// <summary>
        /// Required. The model's resource name. This serves as an ID for the Model to use. This name should match a model name returned by the `ListModels` method. Format: `models/{model}`
        /// </summary>
        public string? Model { get; set; }
        /// <summary>
        /// Required. The content to embed. Only the `parts.text` fields will be counted.
        /// </summary>
        public ContentResponse? Content { get; set; }
        /// <summary>
        /// Optional. Optional task type for which the embeddings will be used. Can only be set for `models/embedding-001`.
        /// </summary>
        public TaskType? TaskType { get; set; }
        /// <summary>
        /// Optional. An optional title for the text. Only applicable when TaskType is `RETRIEVAL_DOCUMENT`. Note: Specifying a `title` for `RETRIEVAL_DOCUMENT` provides better quality embeddings for retrieval.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// Optional. Optional reduced dimension for the output embedding. If set, excessive values in the output embedding are truncated from the end. Supported by newer models since 2024, and the earlier model (`models/embedding-001`) cannot specify this value.
        /// </summary>
        public int? OutputDimensionality { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        public EmbedContentRequest() { }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prompt"></param>
        public EmbedContentRequest(string prompt)
        {
            Content = new() { Text = prompt };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prompts"></param>
        public EmbedContentRequest(List<string> prompts)
        {
            Content = new();
            foreach (var prompt in prompts)
            {
                Content.Parts.Add(new()
                {
                    Text = prompt
                });
            }
        }
    }
}