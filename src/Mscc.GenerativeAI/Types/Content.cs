#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
using System.Text.Json.Serialization;
#endif
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The base structured datatype containing multi-part content of a message.
    /// Ref: https://ai.google.dev/api/rest/v1beta/Content
    /// </summary>
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class Content
    {
        private List<Part>? partTypes;
        
        /// <summary>
        /// Ordered Parts that constitute a single message. Parts may have different MIME types.
        /// </summary>
        [JsonIgnore]
        public List<IPart>? Parts { get; set; }
        /// <summary>
        /// Optional. The producer of the content. Must be either 'user' or 'model'.
        /// Useful to set for multi-turn conversations, otherwise can be left blank or unset.
        /// </summary>
        [JsonPropertyOrder(-1)]
        public string? Role { get; set; }

        /// <summary>
        /// Ordered Parts that constitute a single message. Parts may have different MIME types.
        /// </summary>
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

        public virtual string Text => Parts[0].Text;

        private string GetDebuggerDisplay()
        {
            return $"Role: {Role} - Parts: {Parts.Count}";
        }
    }
}