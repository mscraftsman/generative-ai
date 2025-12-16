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
	/// Config for authentication with API key.
	/// </summary>
	public partial class AuthConfigApiKeyConfig
	{
		/// <summary>
		/// Optional. The name of the SecretManager secret version resource storing the API key. Format: <c>projects/{project}/secrets/{secrete}/versions/{version}</c> - If both <c>api_key_secret</c> and <c>api_key_string</c> are specified, this field takes precedence over <c>api_key_string</c>. - If specified, the <c>secretmanager.versions.access</c> permission should be granted to Vertex AI Extension Service Agent (https://cloud.google.com/vertex-ai/docs/general/access-control#service-agents) on the specified resource.
		/// </summary>
		public string? ApiKeySecret { get; set; }
		/// <summary>
		/// Optional. The API key to be used in the request directly.
		/// </summary>
		public string? ApiKeyString { get; set; }
		/// <summary>
		/// Optional. The location of the API key.
		/// </summary>
		public HttpElementLocation? HttpElementLocation { get; set; }
		/// <summary>
		/// Optional. The parameter name of the API key. E.g. If the API request is &quot;https://example.com/act?api_key=&quot;, &quot;api_key&quot; would be the parameter name.
		/// </summary>
		public string? Name { get; set; }
    }
}