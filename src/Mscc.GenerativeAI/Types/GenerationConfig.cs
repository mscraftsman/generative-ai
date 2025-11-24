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
	/// Configuration options for model generation and outputs. Not all parameters are configurable for every model.
	/// </summary>
	public partial class GenerationConfig : BaseConfig
	{
		/// <summary>
		/// Optional. Config for model selection.
		/// </summary>
		public ModelSelectionConfig? ModelSelectionConfig { get; set; }
		/// <summary>
		/// Optional. An internal detail. Use `responseJsonSchema` rather than this field.
		/// </summary>
		public object? ResponseJsonSchemaOrdered { get; set; }
    }
}