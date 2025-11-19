namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Message to be sent in the first (and only in the first) `BidiGenerateContentClientMessage`. Contains configuration that will apply for the duration of the streaming RPC. Clients should wait for a `BidiGenerateContentSetupComplete` message before sending any additional messages.
    /// </summary>
    public class BidiGenerateContentSetup
    {
        /// <summary>
        /// Required. The model's resource name. This serves as an ID for the Model to use. Format: `models/{model}`
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// Optional. The user provided system instructions for the model. Note: Only text should be used in parts and content in each part will be in a separate paragraph.
        /// </summary>
        public Content? SystemInstruction { get; set; }
        /// <summary>
        /// Optional. A list of `Tools` the model may use to generate the next response. A `Tool` is a piece of code that enables the system to interact with external systems to perform an action, or set of actions, outside of knowledge and scope of the model.
        /// </summary>
        public Tools? Tools { get; set; }
        /// <summary>
        /// Optional. Generation config. The following fields are not supported: - `response_logprobs` - `response_mime_type` - `logprobs` - `response_schema` - `response_json_schema` - `stop_sequence` - `routing_config` - `audio_timestamp`
        /// </summary>
        public GenerationConfig? GenerationConfig { get; set; }
        /// <summary>
        /// Optional. Configures a context window compression mechanism. If included, the server will automatically reduce the size of the context when it exceeds the configured length.
        /// </summary>
        public ContextWindowCompressionConfig? ContextWindowCompression { get; set; }
        /// <summary>
        /// Optional. If set, enables transcription of voice input. The transcription aligns with the input audio language, if configured.
        /// </summary>
        public AudioTranscriptionConfig? InputAudioTranscription { get; set; }
        /// <summary>
        /// Optional. If set, enables transcription of the model's audio output. The transcription aligns with the language code specified for the output audio, if configured.
        /// </summary>
        public AudioTranscriptionConfig? OutputAudioTranscription { get; set; }
        /// <summary>
        /// Optional. Configures the proactivity of the model. This allows the model to respond proactively to the input and to ignore irrelevant input. 
        /// </summary>
        public ProactivityConfig? Proactivity { get; set; }
        /// <summary>
        /// Optional. Configures the handling of realtime input.
        /// </summary>
        public RealtimeInputConfig? RealtimeInputConfig { get; set; }
        /// <summary>
        /// Optional. Configures session resumption mechanism. If included, the server will send `SessionResumptionUpdate` messages.
        /// </summary>
        public SessionResumptionConfig? SessionResumption { get; set; }
    }
}