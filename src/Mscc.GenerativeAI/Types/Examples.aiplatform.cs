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
	/// Example-based explainability that returns the nearest neighbors from the provided dataset.
	/// </summary>
	public partial class Examples
	{
		/// <summary>
		/// The Cloud Storage input instances.
		/// </summary>
		public ExamplesExampleGcsSource? ExampleGcsSource { get; set; }
		/// <summary>
		/// The Cloud Storage locations that contain the instances to be indexed for approximate nearest neighbor search.
		/// </summary>
		public GcsSource? GcsSource { get; set; }
		/// <summary>
		/// The full configuration for the generated index, the semantics are the same as metadata and should match [NearestNeighborSearchConfig](https://cloud.google.com/vertex-ai/docs/explainable-ai/configuring-explanations-example-based#nearest-neighbor-search-config).
		/// </summary>
		public object? NearestNeighborSearchConfig { get; set; }
		/// <summary>
		/// The number of neighbors to return when querying for examples.
		/// </summary>
		public int? NeighborCount { get; set; }
		/// <summary>
		/// Simplified preset configuration, which automatically sets configuration values based on the desired query speed-precision trade-off and modality.
		/// </summary>
		public Presets? Presets { get; set; }
    }
}