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
        /// <summary>
        /// 
        /// </summary>
        public string? Model { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ContentResponse? Content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TaskType? TaskType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Title { get; set; }

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