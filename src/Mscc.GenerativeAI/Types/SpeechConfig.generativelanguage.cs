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
	/// The speech generation config.
	/// </summary>
	public partial class SpeechConfig
	{
		/// <summary>
		/// Optional. Language code (in BCP 47 format, e.g. &quot;en-US&quot;) for speech synthesis. Valid values are: de-DE, en-AU, en-GB, en-IN, en-US, es-US, fr-FR, hi-IN, pt-BR, ar-XA, es-ES, fr-CA, id-ID, it-IT, ja-JP, tr-TR, vi-VN, bn-IN, gu-IN, kn-IN, ml-IN, mr-IN, ta-IN, te-IN, nl-NL, ko-KR, cmn-CN, pl-PL, ru-RU, and th-TH.
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