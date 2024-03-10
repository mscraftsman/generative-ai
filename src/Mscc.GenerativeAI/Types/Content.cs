using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class Content
    {
        private List<Part>? partTypes;
        
        [JsonIgnore]
        public List<IPart>? Parts { get; set; }
        public string? Role { get; set; }

        [DebuggerHidden]
        [JsonPropertyName("parts")]
        public virtual List<Part>? PartTypes 
        { 
            get
            {
                SynchronizeParts();
                return partTypes;
            }
            set => partTypes = value;
        }

        private void SynchronizeParts()
        {
            // partTypes = null;
            if (Parts == null) return;

            partTypes = new List<Part>();
            foreach (var part in Parts)
            {
                if (part is TextData text)
                {
                    partTypes.Add(new Part { TextData = text });
                }
                if (part is InlineData inline)
                {
                    partTypes.Add(new Part { InlineData = inline });
                }
                if (part is FileData file)
                {
                    partTypes.Add(new Part { FileData = file });
                }
                if (part is FunctionResponse response)
                {
                    partTypes.Add(new Part { FunctionResponse = response });
                }
                if (part is FunctionCall call)
                {
                    partTypes.Add(new Part { FunctionCall = call });
                }
            }
        }

        private string GetDebuggerDisplay()
        {
            return $"Role: {Role} - Parts: {Parts?.Count}";
        }
    }

    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class ContentResponse
    {
        public List<Part> Parts { get; set; }
        public string Role { get; set; }

        private string GetDebuggerDisplay()
        {
            return $"Role: {Role} - Parts: {Parts.Count}";
        }
    }
}