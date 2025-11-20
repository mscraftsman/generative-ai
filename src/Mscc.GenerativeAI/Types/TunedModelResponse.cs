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
using System;
using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A fine-tuned model created using ModelService.CreateTunedModel.
	/// </summary>
	public partial class TunedModelResponse
	{
		/// <summary>
		/// Immutable. The name of the <see cref="Model"/> to tune. Example: <see cref="models/gemini-1.5-flash-001"/>
		/// </summary>
		public string? BaseModel { get; set; }
		/// <summary>
		/// Output only. The timestamp when this model was created.
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// Optional. A short description of this model.
		/// </summary>
		public string? Description { get; set; }
		/// <summary>
		/// Optional. The name to display for this model in user interfaces. The display name must be up to 40 characters including spaces.
		/// </summary>
		public string? DisplayName { get; set; }
		/// <summary>
		/// Output only. The tuned model name. A unique name will be generated on create. Example: <see cref="tunedModels/az2mb0bpw6i"/> If display_name is set on create, the id portion of the name will be set by concatenating the words of the display_name with hyphens and adding a random portion for uniqueness. Example: * display_name = <see cref="Sentence Translator"/> * name = <see cref="tunedModels/sentence-translator-u3b7m"/>
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Optional. List of project numbers that have read access to the tuned model.
		/// </summary>
		public List<long>? ReaderProjectNumbers { get; set; }
		/// <summary>
		/// Optional. Controls the randomness of the output. Values can range over <see cref="[0.0,1.0]"/>, inclusive. A value closer to <see cref="1.0"/> will produce responses that are more varied, while a value closer to <see cref="0.0"/> will typically result in less surprising responses from the model. This value specifies default to be the one used by the base model while creating the model.
		/// </summary>
		public double? Temperature { get; set; }
		/// <summary>
		/// Optional. For Top-k sampling. Top-k sampling considers the set of <see cref="top_k"/> most probable tokens. This value specifies default to be used by the backend while making the call to the model. This value specifies default to be the one used by the base model while creating the model.
		/// </summary>
		public int? TopK { get; set; }
		/// <summary>
		/// Optional. For Nucleus sampling. Nucleus sampling considers the smallest set of tokens whose probability sum is at least <see cref="top_p"/>. This value specifies default to be the one used by the base model while creating the model.
		/// </summary>
		public double? TopP { get; set; }
		/// <summary>
		/// Optional. TunedModel to use as the starting point for training the new model.
		/// </summary>
		public TunedModelSource? TunedModelSource { get; set; }
		/// <summary>
		/// Required. The tuning task that creates the tuned model.
		/// </summary>
		public TuningTask? TuningTask { get; set; }
		/// <summary>
		/// Output only. The timestamp when this model was updated.
		/// </summary>
		public DateTime? UpdateTime { get; set; }
    }
}