#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
using System.Text.Json.Serialization;
#endif
using System;
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The base structured datatype containing multi-part content of a message.
    /// Ref: https://ai.google.dev/api/rest/v1beta/Content
    /// </summary>
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public sealed class Content
    {
        private List<Part>? _partTypes;
        
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
        public List<Part>? PartTypes 
        { 
            get
            {
                SynchronizeParts();
                return _partTypes;
            }
            set => _partTypes = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Content"/> class.
        /// </summary>
        internal Content() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Content"/> class.
        /// </summary>
        /// <param name="text">String to process.</param>
        public Content(string text) : this()
        {
            Parts = new List<IPart>();
            Parts.Add(new TextData { Text = text });
        }

        private void SynchronizeParts()
        {
            // partTypes = null;
            if (Parts == null) return;

            _partTypes = new List<Part>();
            foreach (var part in Parts)
            {
                if (part is TextData text)
                {
                    _partTypes.Add(new Part { TextData = text });
                }
                if (part is InlineData inline)
                {
                    _partTypes.Add(new Part { InlineData = inline });
                }
                if (part is FileData file)
                {
                    _partTypes.Add(new Part { FileData = file });
                }
                if (part is FunctionResponse response)
                {
                    _partTypes.Add(new Part { FunctionResponse = response });
                }
                if (part is FunctionCall call)
                {
                    _partTypes.Add(new Part { FunctionCall = call });
                }
                if (part is VideoMetadata video)
                {
                    _partTypes.Add(new Part { VideoMetadata = video });
                }
            }
        }

        private string GetDebuggerDisplay()
        {
            return $"Role: {Role} - Parts: {Parts?.Count}";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public sealed class ContentResponse
    {
        public List<Part> Parts { get; set; }
        public string Role { get; set; }

        [JsonIgnore]
        public string Text
        {
            get
            {
                if (Parts.Count > 0)
                {
                    return Parts[0].Text;
                }
                return string.Empty;
            }
            set
            {
                if (Parts.Count == 0)
                {
                    Parts.Add(new Part() { Text = value });
                }
                else
                {
                    Parts[0].Text = value;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentResponse"/> class.
        /// </summary>
        internal ContentResponse() => Parts = new List<Part>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentResponse"/> class.
        /// </summary>
        /// <param name="text">String to process.</param>
        /// <param name="role">Role of the content. Must be either 'user' or 'model'.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> or <paramref name="role"/> is null.</exception>
        public ContentResponse(string text, string role = GenerativeAI.Role.User) : this()
        {
            // if (text == null) throw new ArgumentNullException(nameof(text));
            // if (role == null) throw new ArgumentNullException(nameof(role));

            Parts.Add(new Part() { Text = text });
            Role = role;
        }

        private string GetDebuggerDisplay()
        {
            return $"Role: {Role} - Parts: {Parts.Count}";
        }
    }
}