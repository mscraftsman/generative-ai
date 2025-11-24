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
using System.Collections.Generic;
using System.Text.Json.Serialization;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Specification for how rubrics should be generated.
	/// </summary>
	public partial class RubricGenerationSpec
	{
		/// <summary>
		/// Configuration for the model used in rubric generation. Configs including sampling count and base model can be specified here. Flipping is not supported for rubric generation.
		/// </summary>
		public AutoraterConfig? ModelConfig { get; set; }
		/// <summary>
		/// Template for the prompt used to generate rubrics. The details should be updated based on the most-recent recipe requirements.
		/// </summary>
		public string? PromptTemplate { get; set; }
		/// <summary>
		/// The type of rubric content to be generated.
		/// </summary>
		public RubricGenerationSpecRubricContentType? RubricContentType { get; set; }
		/// <summary>
		/// Optional. An optional, pre-defined list of allowed types for generated rubrics. If this field is provided, it implies <c>include_rubric_type</c> should be true, and the generated rubric types should be chosen from this ontology.
		/// </summary>
		public List<string>? RubricTypeOntology { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<RubricGenerationSpecRubricContentType>))]
		public enum RubricGenerationSpecRubricContentType
		{
			/// <summary>
			/// The content type to generate is not specified.
			/// </summary>
			RubricContentTypeUnspecified,
			/// <summary>
			/// Generate rubrics based on properties.
			/// </summary>
			Property,
			/// <summary>
			/// Generate rubrics in an NL question answer format.
			/// </summary>
			NlQuestionAnswer,
			/// <summary>
			/// Generate rubrics in a unit test format.
			/// </summary>
			PythonCodeAssertion,
		}
    }
}