#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif
using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class EmbedContentRequest
    {
        public string Model { get; } = $"{GenerativeAI.Model.Embedding.SanitizeModelName()}";

        /// <summary>
        /// 
        /// </summary>
        public ContentResponse Content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TaskType? TaskType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Title { get; set; }

        public EmbedContentRequest(string prompt)
        {
            Content = new()
            {
                Parts = new List<Part> { new()
                {
                    Text = prompt
                }}
            };
        }

        public EmbedContentRequest(List<string> prompts)
        {
            Content = new()
            {
                Parts = new List<Part>()
            };
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