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
	/// Configuration for content generation. This message contains all the parameters that control how the model generates content. It allows you to influence the randomness, length, and structure of the output.
	/// </summary>
	public partial class GenerationConfig
	{

		/// <summary>
		/// Optional. If enabled, audio timestamps will be included in the request to the model. This can be useful for synchronizing audio with other modalities in the response.
		/// </summary>
		public bool? AudioTimestamp { get; set; }
		/// <summary>
		/// Optional. If enabled, the model will detect emotions and adapt its responses accordingly. For example, if the model detects that the user is frustrated, it may provide a more empathetic response.
		/// </summary>
		public bool? EnableAffectiveDialog { get; set; }
		/// <summary>
		/// Optional. Config for model selection.
		/// </summary>
		public GenerationConfigModelConfig? ModelConfig { get; set; }
		/// <summary>
		/// Optional. Routing configuration.
		/// </summary>
		public GenerationConfigRoutingConfig? RoutingConfig { get; set; }
    }
}