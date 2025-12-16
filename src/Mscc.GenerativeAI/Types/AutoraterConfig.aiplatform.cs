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
	/// The configs for autorater. This is applicable to both EvaluateInstances and EvaluateDataset.
	/// </summary>
	public partial class AutoraterConfig
	{
		/// <summary>
		/// Optional. The fully qualified name of the publisher model or tuned autorater endpoint to use. Publisher model format: <c>projects/{project}/locations/{location}/publishers/*/models/*</c> Tuned model endpoint format: <c>projects/{project}/locations/{location}/endpoints/{endpoint}</c>
		/// </summary>
		public string? AutoraterModel { get; set; }
		/// <summary>
		/// Optional. Default is true. Whether to flip the candidate and baseline responses. This is only applicable to the pairwise metric. If enabled, also provide PairwiseMetricSpec.candidate_response_field_name and PairwiseMetricSpec.baseline_response_field_name. When rendering PairwiseMetricSpec.metric_prompt_template, the candidate and baseline fields will be flipped for half of the samples to reduce bias.
		/// </summary>
		public bool? FlipEnabled { get; set; }
		/// <summary>
		/// Optional. Configuration options for model generation and outputs.
		/// </summary>
		public GenerationConfig? GenerationConfig { get; set; }
		/// <summary>
		/// Optional. Number of samples for each instance in the dataset. If not specified, the default is 4. Minimum value is 1, maximum value is 32.
		/// </summary>
		public int? SamplingCount { get; set; }
    }
}