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
using System.Text.Json.Serialization;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Configures automatic detection of activity.
	/// </summary>
	public partial class AutomaticActivityDetection
	{
		/// <summary>
		/// Optional. If enabled (the default), detected voice and text input count as activity. If disabled, the client must send activity signals.
		/// </summary>
		public bool? Disabled { get; set; }
		/// <summary>
		/// Optional. Determines how likely detected speech is ended.
		/// </summary>
		public EndOfSpeechSensitivityType? EndOfSpeechSensitivity { get; set; }
		/// <summary>
		/// Optional. The required duration of detected speech before start-of-speech is committed. The lower this value, the more sensitive the start-of-speech detection is and shorter speech can be recognized. However, this also increases the probability of false positives.
		/// </summary>
		public int? PrefixPaddingMs { get; set; }
		/// <summary>
		/// Optional. The required duration of detected non-speech (e.g. silence) before end-of-speech is committed. The larger this value, the longer speech gaps can be without interrupting the user&apos;s activity but this will increase the model&apos;s latency.
		/// </summary>
		public int? SilenceDurationMs { get; set; }
		/// <summary>
		/// Optional. Determines how likely speech is to be detected.
		/// </summary>
		public StartOfSpeechSensitivityType? StartOfSpeechSensitivity { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<EndOfSpeechSensitivityType>))]
		public enum EndOfSpeechSensitivityType
		{
			/// <summary>
			/// The default is END_SENSITIVITY_HIGH.
			/// </summary>
			EndSensitivityUnspecified,
			/// <summary>
			/// Automatic detection ends speech more often.
			/// </summary>
			EndSensitivityHigh,
			/// <summary>
			/// Automatic detection ends speech less often.
			/// </summary>
			EndSensitivityLow,
		}

		[JsonConverter(typeof(JsonStringEnumConverter<StartOfSpeechSensitivityType>))]
		public enum StartOfSpeechSensitivityType
		{
			/// <summary>
			/// The default is START_SENSITIVITY_HIGH.
			/// </summary>
			StartSensitivityUnspecified,
			/// <summary>
			/// Automatic detection will detect the start of speech more often.
			/// </summary>
			StartSensitivityHigh,
			/// <summary>
			/// Automatic detection will detect the start of speech less often.
			/// </summary>
			StartSensitivityLow,
		}
    }
}