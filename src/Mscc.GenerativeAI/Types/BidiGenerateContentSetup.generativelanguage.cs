/*
 * Copyleft 2024-2025 Jochen Kirstätter and contributors
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      https://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Message to be sent in the first (and only in the first) <c>BidiGenerateContentClientMessage</c>. Contains configuration that will apply for the duration of the streaming RPC. Clients should wait for a <c>BidiGenerateContentSetupComplete</c> message before sending any additional messages.
	/// </summary>
	public partial class BidiGenerateContentSetup
	{
		/// <summary>
		/// Optional. Configures a context window compression mechanism. If included, the server will automatically reduce the size of the context when it exceeds the configured length.
		/// </summary>
		public ContextWindowCompressionConfig? ContextWindowCompression { get; set; }
		/// <summary>
		/// Optional. Generation config. The following fields are not supported: - <c>response_logprobs</c> - <c>response_mime_type</c> - <c>logprobs</c> - <c>response_schema</c> - <c>response_json_schema</c> - <c>stop_sequence</c> - <c>routing_config</c> - <c>audio_timestamp</c>
		/// </summary>
		public GenerationConfig? GenerationConfig { get; set; }
		/// <summary>
		/// Optional. If set, enables transcription of voice input. The transcription aligns with the input audio language, if configured.
		/// </summary>
		public AudioTranscriptionConfig? InputAudioTranscription { get; set; }
		/// <summary>
		/// Required. The model&apos;s resource name. This serves as an ID for the Model to use. Format: <c>models/{model}</c>
		/// </summary>
		public string? Model { get; set; }
		/// <summary>
		/// Optional. If set, enables transcription of the model&apos;s audio output. The transcription aligns with the language code specified for the output audio, if configured.
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
		/// Optional. Configures session resumption mechanism. If included, the server will send <c>SessionResumptionUpdate</c> messages.
		/// </summary>
		public SessionResumptionConfig? SessionResumption { get; set; }
		/// <summary>
		/// Optional. The user provided system instructions for the model. Note: Only text should be used in parts and content in each part will be in a separate paragraph.
		/// </summary>
		public Content? SystemInstruction { get; set; }
		/// <summary>
		/// Optional. A list of <c>Tools</c> the model may use to generate the next response. A <c>Tool</c> is a piece of code that enables the system to interact with external systems to perform an action, or set of actions, outside of knowledge and scope of the model.
		/// </summary>
		public Tools? Tools { get; set; }
    }
}