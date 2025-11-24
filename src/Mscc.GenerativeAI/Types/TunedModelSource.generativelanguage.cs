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
	/// Tuned model as a source for training a new model.
	/// </summary>
	public partial class TunedModelSource
	{
		/// <summary>
		/// Output only. The name of the base <c>Model</c> this <c>TunedModel</c> was tuned from. Example: <c>models/gemini-1.5-flash-001</c>
		/// </summary>
		public string? BaseModel { get; set; }
		/// <summary>
		/// Immutable. The name of the <c>TunedModel</c> to use as the starting point for training the new model. Example: <c>tunedModels/my-tuned-model</c>
		/// </summary>
		public string? TunedModel { get; set; }
    }
}