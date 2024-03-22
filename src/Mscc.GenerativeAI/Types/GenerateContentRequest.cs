#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
using System.Linq;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class GenerateContentRequest
    {
        /// <summary>
        /// Required. The content of the current conversation with the model.
        /// For single-turn queries, this is a single instance. For multi-turn queries, this is a repeated field that contains conversation history + latest request.
        /// </summary>
        public List<Content>? Contents { get; set; }
        /// <summary>
        /// Optional. Configuration options for model generation and outputs.
        /// </summary>
        public GenerationConfig? GenerationConfig { get; set; }
        /// <summary>
        /// Optional. A list of unique SafetySetting instances for blocking unsafe content.
        /// This will be enforced on the GenerateContentRequest.contents and GenerateContentResponse.candidates. There should not be more than one setting for each SafetyCategory type. The API will block any contents and responses that fail to meet the thresholds set by these settings. This list overrides the default settings for each SafetyCategory specified in the safetySettings. If there is no SafetySetting for a given SafetyCategory provided in the list, the API will use the default safety setting for that category. Harm categories HARM_CATEGORY_HATE_SPEECH, HARM_CATEGORY_SEXUALLY_EXPLICIT, HARM_CATEGORY_DANGEROUS_CONTENT, HARM_CATEGORY_HARASSMENT are supported.
        /// </summary>
        public List<SafetySetting>? SafetySettings { get; set; }
        /// <summary>
        /// Optional. A list of Tools the model may use to generate the next response.
        /// A Tool is a piece of code that enables the system to interact with external systems to perform an action, or set of actions, outside of knowledge and scope of the model. The only supported tool is currently Function.
        /// </summary>
        public List<Tool>? Tools { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GenerateContentRequest() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="generationConfig"></param>
        /// <param name="safetySettings"></param>
        /// <param name="tools"></param>
        public GenerateContentRequest(string prompt,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null) : this()
        {
            Contents = new List<Content> { new Content
            {
                Role = Role.User,
                Parts = new List<IPart> { new TextData
                {
                    Text = prompt
                }}
            }};
            if (generationConfig != null) GenerationConfig = generationConfig;
            if (safetySettings != null) SafetySettings = safetySettings;
            if (tools != null) Tools = tools;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="generationConfig"></param>
        /// <param name="safetySettings"></param>
        /// <param name="tools"></param>
        public GenerateContentRequest(List<IPart> parts,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null)
        {
            Contents = new List<Content> { new Content
            {
                Parts = parts
            }};
            if (generationConfig != null) GenerationConfig = generationConfig;
            if (safetySettings != null) SafetySettings = safetySettings;
            if (tools != null) Tools = tools;
        }

        public GenerateContentRequest(List<Part> parts,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null)
        {
            Contents = new List<Content> { new Content
            {
                Parts = parts.Select(p => (IPart)p).ToList()
            }};
            if (generationConfig != null) GenerationConfig = generationConfig;
            if (safetySettings != null) SafetySettings = safetySettings;
            if (tools != null) Tools = tools;
        }
    }
}