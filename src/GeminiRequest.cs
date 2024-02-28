using System.Collections.Generic;
using System.Linq;

namespace Mscc.GenerativeAI
{
    // Todo: Integrate GenerationConfig, SafetySettings, and Tools.
    public class GeminiRequest
    {
        public List<Content>? Contents { get; set; }
        public List<SafetySetting>? SafetySettings { get; set; }
        public GenerationConfig? GenerationConfig { get; set; }
        public List<Tool>? Tools { get; set; }

        public GeminiRequest() { }

        public GeminiRequest(string prompt)
        {
            Contents = new List<Content> { new Content
            {
                Parts = new List<IPart> { new TextData
                {
                    Text = prompt
                }}
            }};
        }

        public GeminiRequest(List<IPart> parts)
        {
            Contents = new List<Content> { new Content
            {
                Parts = parts
            }};
        }

        public GeminiRequest(List<Part> parts)
        {
            Contents = new List<Content> { new Content
            {
                Parts = parts.Select(p => (IPart)p).ToList()
            }};
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