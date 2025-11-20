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
	/// Tuned model as a source for training a new model.
	/// </summary>
	public partial class TunedModelSource
	{
		/// <summary>
		/// Output only. The name of the base <see cref="Model"/> this <see cref="TunedModel"/> was tuned from. Example: <see cref="models/gemini-1.5-flash-001"/>
		/// </summary>
		public string? BaseModel { get; set; }
		/// <summary>
		/// Immutable. The name of the <see cref="TunedModel"/> to use as the starting point for training the new model. Example: <see cref="tunedModels/my-tuned-model"/>
		/// </summary>
		public string? TunedModel { get; set; }
    }
}