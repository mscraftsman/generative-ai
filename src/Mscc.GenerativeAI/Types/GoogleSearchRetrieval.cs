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
	/// Tool to retrieve public web data for grounding, powered by Google.
	/// </summary>
	public partial class GoogleSearchRetrieval : ITool
	{
		/// <summary>
		/// Optional. Disable using the result from this tool in detecting grounding attribution.
		/// </summary>
		/// <remarks>This does not affect how the result is given to the model for generation.</remarks>
		public bool? DisableAttribution { get; set; }

		/// <summary>
		/// Creates an instance of <see cref="GoogleSearchRetrieval"/>
		/// </summary>
		public GoogleSearchRetrieval() 
			: this(DynamicRetrievalConfigMode.ModeDynamic, 0.3f) { }
        
		/// <summary>
		/// Creates an instance of <see cref="GoogleSearchRetrieval"/> with Mode and DynamicThreshold.
		/// </summary>
		/// <param name="mode">The mode of the predictor to be used in dynamic retrieval.</param>
		/// <param name="dynamicThreshold">The threshold to be used in dynamic retrieval. If not set, a system default value is used.</param>
		public GoogleSearchRetrieval(DynamicRetrievalConfigMode mode, float dynamicThreshold)
		{
			DynamicRetrievalConfig = new DynamicRetrievalConfig
			{
				Mode = mode, 
				DynamicThreshold = dynamicThreshold
			};
		}
	}
}