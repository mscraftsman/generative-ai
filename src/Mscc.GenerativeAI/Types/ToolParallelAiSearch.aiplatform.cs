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
	/// ParallelAiSearch tool type. A tool that uses the Parallel.ai search engine for grounding.
	/// </summary>
	public partial class ToolParallelAiSearch
	{
		/// <summary>
		/// Optional. The API key for ParallelAiSearch. If an API key is not provided, the system will attempt to verify access by checking for an active Parallel.ai subscription through the Google Cloud Marketplace. See https://docs.parallel.ai/search/search-quickstart for more details.
		/// </summary>
		public string? ApiKey { get; set; }
		/// <summary>
		/// Optional. Custom configs for ParallelAiSearch. This field can be used to pass any parameter from the Parallel.ai Search API. See the Parallel.ai documentation for the full list of available parameters and their usage: https://docs.parallel.ai/api-reference/search-beta/search Currently only <c>source_policy</c>, <c>excerpts</c>, <c>max_results</c>, <c>mode</c>, <c>fetch_policy</c> can be set via this field. For example: { &quot;source_policy&quot;: { &quot;include_domains&quot;: [&quot;google.com&quot;, &quot;wikipedia.org&quot;], &quot;exclude_domains&quot;: [&quot;example.com&quot;] }, &quot;fetch_policy&quot;: { &quot;max_age_seconds&quot;: 3600 } }
		/// </summary>
		public object? CustomConfigs { get; set; }
    }
}