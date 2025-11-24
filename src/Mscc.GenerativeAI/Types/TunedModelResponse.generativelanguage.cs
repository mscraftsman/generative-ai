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
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A fine-tuned model created using ModelService.CreateTunedModel.
	/// </summary>
	public partial class TunedModelResponse
	{
		/// <summary>
		/// Immutable. The name of the <c>Model</c> to tune. Example: <c>models/gemini-1.5-flash-001</c>
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
		/// Output only. The tuned model name. A unique name will be generated on create. Example: <c>tunedModels/az2mb0bpw6i</c> If display_name is set on create, the id portion of the name will be set by concatenating the words of the display_name with hyphens and adding a random portion for uniqueness. Example: * display_name = <c>Sentence Translator</c> * name = <c>tunedModels/sentence-translator-u3b7m</c>
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Optional. List of project numbers that have read access to the tuned model.
		/// </summary>
		public List<long>? ReaderProjectNumbers { get; set; }
		/// <summary>
		/// Output only. The state of the tuned model.
		/// </summary>
		public StateType? State { get; set; }
		/// <summary>
		/// Optional. Controls the randomness of the output. Values can range over <c>[0.0,1.0]</c>, inclusive. A value closer to <c>1.0</c> will produce responses that are more varied, while a value closer to <c>0.0</c> will typically result in less surprising responses from the model. This value specifies default to be the one used by the base model while creating the model.
		/// </summary>
		public double? Temperature { get; set; }
		/// <summary>
		/// Optional. For Top-k sampling. Top-k sampling considers the set of <c>top_k</c> most probable tokens. This value specifies default to be used by the backend while making the call to the model. This value specifies default to be the one used by the base model while creating the model.
		/// </summary>
		public int? TopK { get; set; }
		/// <summary>
		/// Optional. For Nucleus sampling. Nucleus sampling considers the smallest set of tokens whose probability sum is at least <c>top_p</c>. This value specifies default to be the one used by the base model while creating the model.
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

		[JsonConverter(typeof(JsonStringEnumConverter<StateType>))]
		public enum StateType
		{
			/// <summary>
			/// The default value. This value is unused.
			/// </summary>
			StateUnspecified,
			/// <summary>
			/// The model is being created.
			/// </summary>
			Creating,
			/// <summary>
			/// The model is ready to be used.
			/// </summary>
			Active,
			/// <summary>
			/// The model failed to be created.
			/// </summary>
			Failed,
		}
    }
}