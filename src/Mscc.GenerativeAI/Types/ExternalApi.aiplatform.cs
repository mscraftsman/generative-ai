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
using System.Text.Json.Serialization;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Retrieve from data source powered by external API for grounding. The external API is not owned by Google, but need to follow the pre-defined API spec.
	/// </summary>
	public partial class ExternalApi
	{
		/// <summary>
		/// The authentication config to access the API. Deprecated. Please use auth_config instead.
		/// </summary>
		public ApiAuth? ApiAuth { get; set; }
		/// <summary>
		/// The API spec that the external API implements.
		/// </summary>
		public ApiSpecType? ApiSpec { get; set; }
		/// <summary>
		/// The authentication config to access the API.
		/// </summary>
		public AuthConfig? AuthConfig { get; set; }
		/// <summary>
		/// Parameters for the elastic search API.
		/// </summary>
		public ExternalApiElasticSearchParams? ElasticSearchParams { get; set; }
		/// <summary>
		/// The endpoint of the external API. The system will call the API at this endpoint to retrieve the data for grounding. Example: https://acme.com:443/search
		/// </summary>
		public string? Endpoint { get; set; }
		/// <summary>
		/// Parameters for the simple search API.
		/// </summary>
		public ExternalApiSimpleSearchParams? SimpleSearchParams { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<ApiSpecType>))]
		public enum ApiSpecType
		{
			/// <summary>
			/// Unspecified API spec. This value should not be used.
			/// </summary>
			ApiSpecUnspecified,
			/// <summary>
			/// Simple search API spec.
			/// </summary>
			SimpleSearch,
			/// <summary>
			/// Elastic search API spec.
			/// </summary>
			ElasticSearch,
		}
    }
}