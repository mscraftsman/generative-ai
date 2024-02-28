using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    public class ChatSession
    {
        public List<Content>? History { get; set; }
        public List<Tool>? Tools { get; set; }


        public GenerateContentResponse SendMessage(string content, GenerationConfig? generationConfig, List<SafetySetting>? safetySettings)
        {
            return new GenerateContentResponse();
        }

        public List<GenerateContentResponse> SendMessageStream(string content, GenerationConfig? generationConfig, List<SafetySetting>? safetySettings)
        {
            return new List<GenerateContentResponse>();
        }
    }
}