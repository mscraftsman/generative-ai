using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Optional model configuration parameters.
    /// </summary>
    public partial class GenerateContentConfig : GenerationConfig
    {
        /// <summary>
        /// If enabled, audio timestamp will be included in the request to the model.
        /// </summary>
        public bool? AudioTimestamp { get; set; }
        /// <summary>
        /// The configuration for automatic function calling.
        /// </summary>
        public AutomaticFunctionCallingConfig? AutomaticFunctionCalling { get; set; }
        /// <summary>
        /// Resource name of a context cache that can be used in subsequent requests.
        /// </summary>
        public string? CachedContent { get; set; }
        /// <summary>
        /// Labels with user-defined metadata to break down billed charges.
        /// </summary>
        public Dictionary<string, string>? Labels { get; set; }
        /// <summary>
        /// Configuration for model router requests.
        /// </summary>
        public RoutingConfig? RoutingConfig { get; set; }
        /// <summary>
        /// Safety settings in the request to block unsafe content in the response.
        /// </summary>
        public List<SafetySetting>? SafetySettings { get; set; }
        /// <summary>
        /// Instructions for the model to steer it toward better performance. For example, “Answer as concisely as possible” or “Don’t use technical terms in your response”.
        /// </summary>
        public Content? SystemInstruction { get; set; }
        /// <summary>
        /// Associates model output to a specific function call.
        /// </summary>
        public ToolConfig? ToolConfig { get; set; }
        /// <summary>
        /// Code that enables the system to interact with external systems to perform an action outside of the knowledge and scope of the model.
        /// </summary>
        public Tools? Tools { get; set; }
    }
}