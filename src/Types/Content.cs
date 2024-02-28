using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    public class Content
    {
        [JsonIgnore]
        public List<IPart>? Parts { get; set; }
        public string? Role { get; set; }

        [JsonPropertyName("parts")]
        public virtual List<Part>? PartTypes { get; set; }

        internal void SynchronizeParts()
        {
            PartTypes = new List<Part>();
            foreach (var part in Parts)
            {
                if (part is TextData text)
                {
                    PartTypes.Add(new Part { TextData = text });
                }
                if (part is InlineData inline)
                {
                    PartTypes.Add(new Part { InlineData = inline });
                }
                if (part is FileData file)
                {
                    PartTypes.Add(new Part { FileData = file });
                }
                if (part is FunctionResponse response)
                {
                    PartTypes.Add(new Part { FunctionResponse = response });
                }
                if (part is FunctionCall call)
                {
                    PartTypes.Add(new Part { FunctionCall = call });
                }
            }

        }
    }

    public class ContentResponse
    {
        public List<Part> Parts { get; set; }
        public string Role { get; set; }
    }
}