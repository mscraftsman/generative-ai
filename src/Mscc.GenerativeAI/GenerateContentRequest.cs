using System.Collections.Generic;
using System.Linq;

namespace Mscc.GenerativeAI
{
    // Todo: Integrate GenerationConfig, SafetySettings, and Tools.
    public class GenerateContentRequest

    {
        public List<Content>? Contents { get; set; }
        public GenerationConfig? GenerationConfig { get; set; }
        public List<SafetySetting>? SafetySettings { get; set; }
        public List<Tool>? Tools { get; set; }

        public GenerateContentRequest() { }

        public GenerateContentRequest(string prompt, GenerationConfig? generationConfig = null, List<SafetySetting>? safetySettings = null, List<Tool>? tools = null)
        {
            Contents = new List<Content> { new Content
            {
                Parts = new List<IPart> { new TextData
                {
                    Text = prompt
                }}
            }};
            if (generationConfig != null) GenerationConfig = generationConfig;
            if (safetySettings != null) SafetySettings = safetySettings;
            if (tools != null) Tools = tools;
        }

        public GenerateContentRequest(List<IPart> parts, GenerationConfig? generationConfig = null, List<SafetySetting>? safetySettings = null, List<Tool>? tools = null)
        {
            Contents = new List<Content> { new Content
            {
                Parts = parts
            }};
            if (generationConfig != null) GenerationConfig = generationConfig;
            if (safetySettings != null) SafetySettings = safetySettings;
            if (tools != null) Tools = tools;
        }

        public GenerateContentRequest(List<Part> parts, GenerationConfig? generationConfig = null, List<SafetySetting>? safetySettings = null, List<Tool>? tools = null)
        {
            Contents = new List<Content> { new Content
            {
                Parts = parts.Select(p => (IPart)p).ToList()
            }};
            if (generationConfig != null) GenerationConfig = generationConfig;
            if (safetySettings != null) SafetySettings = safetySettings;
            if (tools != null) Tools = tools;
        }

        internal void Synchronize()
        {
            foreach (var content in Contents)
            {
                content.SynchronizeParts();
            }
        }
    }
}