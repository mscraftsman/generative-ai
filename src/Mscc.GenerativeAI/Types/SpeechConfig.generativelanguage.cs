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
	/// Config for speech generation and transcription.
	/// </summary>
	public partial class SpeechConfig
	{
		/// <summary>
		/// Optional. The IETF [BCP-47](https://www.rfc-editor.org/rfc/bcp/bcp47.txt) language code that the user configured the app to use. Used for speech recognition and synthesis. Valid values are: <c>de-DE</c>, <c>en-AU</c>, <c>en-GB</c>, <c>en-IN</c>, <c>en-US</c>, <c>es-US</c>, <c>fr-FR</c>, <c>hi-IN</c>, <c>pt-BR</c>, <c>ar-XA</c>, <c>es-ES</c>, <c>fr-CA</c>, <c>id-ID</c>, <c>it-IT</c>, <c>ja-JP</c>, <c>tr-TR</c>, <c>vi-VN</c>, <c>bn-IN</c>, <c>gu-IN</c>, <c>kn-IN</c>, <c>ml-IN</c>, <c>mr-IN</c>, <c>ta-IN</c>, <c>te-IN</c>, <c>nl-NL</c>, <c>ko-KR</c>, <c>cmn-CN</c>, <c>pl-PL</c>, <c>ru-RU</c>, and <c>th-TH</c>.
		/// </summary>
		public string? LanguageCode { get; set; }
		/// <summary>
		/// Optional. The configuration for the multi-speaker setup. It is mutually exclusive with the voice_config field.
		/// </summary>
		public MultiSpeakerVoiceConfig? MultiSpeakerVoiceConfig { get; set; }
		/// <summary>
		/// The configuration in case of single-voice output.
		/// </summary>
		public VoiceConfig? VoiceConfig { get; set; }
    }
}