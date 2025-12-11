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
	/// The configuration for the replicated voice to use.
	/// </summary>
	public partial class ReplicatedVoiceConfig
	{
		/// <summary>
		/// Optional. The mimetype of the voice sample. The only currently supported value is <c>audio/wav</c>. This represents 16-bit signed little-endian wav data, with a 24kHz sampling rate. <c>mime_type</c> will default to <c>audio/wav</c> if not set.
		/// </summary>
		public string? MimeType { get; set; }
		/// <summary>
		/// Optional. The sample of the custom voice.
		/// </summary>
		public byte[]? VoiceSampleAudio { get; set; }
    }
}