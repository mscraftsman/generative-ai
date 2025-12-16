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
	/// Specification for an LLM based metric.
	/// </summary>
	public partial class LLMBasedMetricSpec
	{
		/// <summary>
		/// Optional. Optional additional configuration for the metric.
		/// </summary>
		public object? AdditionalConfig { get; set; }
		/// <summary>
		/// Optional. Optional configuration for the judge LLM (Autorater).
		/// </summary>
		public AutoraterConfig? JudgeAutoraterConfig { get; set; }
		/// <summary>
		/// Required. Template for the prompt sent to the judge model.
		/// </summary>
		public string? MetricPromptTemplate { get; set; }
		/// <summary>
		/// Dynamically generate rubrics using a predefined spec.
		/// </summary>
		public PredefinedMetricSpec? PredefinedRubricGenerationSpec { get; set; }
		/// <summary>
		/// Dynamically generate rubrics using this specification.
		/// </summary>
		public RubricGenerationSpec? RubricGenerationSpec { get; set; }
		/// <summary>
		/// Use a pre-defined group of rubrics associated with the input. Refers to a key in the rubric_groups map of EvaluationInstance.
		/// </summary>
		public string? RubricGroupKey { get; set; }
		/// <summary>
		/// Optional. System instructions for the judge model.
		/// </summary>
		public string? SystemInstruction { get; set; }
    }
}