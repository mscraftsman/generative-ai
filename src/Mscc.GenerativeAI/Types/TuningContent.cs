using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Diagnostics;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// The structured datatype containing multi-part content of an example message. This is a subset of the Content proto used during model inference with limited type support. A `Content` includes a `role` field designating the producer of the `Content` and a `parts` field containing multi-part data that contains the content of the message turn.
    /// </summary>
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public partial class TuningContent
    {
        // 
        private List<TuningPart>? _partTypes;
        
        /// <summary>
        /// Ordered `Parts` that constitute a single message. Parts may have different MIME types.
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
        public List<TuningPart>? PartTypes 
        { 
            get
            {
                SynchronizeParts();
                return _partTypes;
            }
            set => _partTypes = value;
        }
        /// <summary>
        /// The ETag of the item.
        /// </summary>
        public string? ETag { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Content"/> class.
        /// </summary>
        internal TuningContent()
        {
            Parts = new List<IPart>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Content"/> class.
        /// </summary>
        /// <param name="text">String to process.</param>
        public TuningContent(string text) : this()
        {
            Parts?.Add(new TextData { Text = text });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Content"/> class.
        /// </summary>
        /// <param name="file">File to process.</param>
        public TuningContent(FileData file) : this()
        {
            Role = GenerativeAI.Types.Role.User;
            Parts?.Add(new FileData { FileUri = file.FileUri, MimeType = file.MimeType });
        }

        private void SynchronizeParts()
        {
            // partTypes = null;
            if (Parts is null || Parts?.Count == 0) return;

            _partTypes = [];
            foreach (var part in Parts!)
            {
                if (part is TextData text)
                {
                    _partTypes.Add(new TuningPart { TextData = text });
                }
                if (part is InlineData inline)
                {
                    _partTypes.Add(new TuningPart { InlineData = inline });
                }
                if (part is FileData file)
                {
                    _partTypes.Add(new TuningPart { FileData = file });
                }
                if (part is FunctionResponse response)
                {
                    _partTypes.Add(new TuningPart { FunctionResponse = response });
                }
                if (part is FunctionCall call)
                {
                    _partTypes.Add(new TuningPart { FunctionCall = call });
                }
                if (part is VideoMetadata video)
                {
                    _partTypes.Add(new TuningPart { VideoMetadata = video });
                }
                if (part is ExecutableCode code)
                {
                    _partTypes.Add(new TuningPart { ExecutableCode = code });
                }
                if (part is CodeExecutionResult result)
                {
                    _partTypes.Add(new TuningPart { CodeExecutionResult = result });
                }
            }
        }

        private string GetDebuggerDisplay()
        {
            return $"Role: {Role} - Parts: {Parts?.Count}";
        }
    }
}