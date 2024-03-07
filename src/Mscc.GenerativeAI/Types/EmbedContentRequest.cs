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
        public string Model { get; } = $"models/{GenerativeAI.Model.Embedding}";

        /// <summary>
        /// 
        /// </summary>
        public Content Content { get; set; }
        // [JsonPropertyName("generation_config")]
        // public GenerationConfig? GenerationConfig { get; set; }
        // [JsonPropertyName("safety_settings")]
        // public List<SafetySetting>? SafetySettings { get; set; }
        // public List<Tool>? Tools { get; set; }

        public EmbedContentRequest(string prompt, GenerationConfig? generationConfig = null, List<SafetySetting>? safetySettings = null, List<Tool>? tools = null)
        {
            Content = new Content
            {
                Parts = new List<IPart> { new TextData
                {
                    Text = prompt
                }}
            };
            // if (generationConfig != null) GenerationConfig = generationConfig;
            // if (safetySettings != null) SafetySettings = safetySettings;
            // if (tools != null) Tools = tools;
        }
    }
}