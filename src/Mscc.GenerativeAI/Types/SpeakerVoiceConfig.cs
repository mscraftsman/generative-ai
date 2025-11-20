/*
 * Copyright 2024-2025 Jochen Kirstätter
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

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The configuration for a single speaker in a multi speaker setup.
	/// </summary>
	public partial class SpeakerVoiceConfig
	{
		/// <summary>
		/// Required. The name of the speaker to use. Should be the same as in the prompt.
		/// </summary>
		public string? Speaker { get; set; }
		/// <summary>
		/// Required. The configuration for the voice to use.
		/// </summary>
		public VoiceConfig? VoiceConfig { get; set; }
    }
}