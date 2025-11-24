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
	/// Auth configuration to run the extension.
	/// </summary>
	public partial class AuthConfig
	{
		/// <summary>
		/// Config for API key auth.
		/// </summary>
		public AuthConfigApiKeyConfig? ApiKeyConfig { get; set; }
		/// <summary>
		/// Type of auth scheme.
		/// </summary>
		public AuthConfigAuthType? AuthType { get; set; }
		/// <summary>
		/// Config for Google Service Account auth.
		/// </summary>
		public AuthConfigGoogleServiceAccountConfig? GoogleServiceAccountConfig { get; set; }
		/// <summary>
		/// Config for HTTP Basic auth.
		/// </summary>
		public AuthConfigHttpBasicAuthConfig? HttpBasicAuthConfig { get; set; }
		/// <summary>
		/// Config for user oauth.
		/// </summary>
		public AuthConfigOauthConfig? OauthConfig { get; set; }
		/// <summary>
		/// Config for user OIDC auth.
		/// </summary>
		public AuthConfigOidcConfig? OidcConfig { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<AuthConfigAuthType>))]
		public enum AuthConfigAuthType
		{
			AuthTypeUnspecified,
			/// <summary>
			/// No Auth.
			/// </summary>
			NoAuth,
			/// <summary>
			/// API Key Auth.
			/// </summary>
			ApiKeyAuth,
			/// <summary>
			/// HTTP Basic Auth.
			/// </summary>
			HttpBasicAuth,
			/// <summary>
			/// Google Service Account Auth.
			/// </summary>
			GoogleServiceAccountAuth,
			/// <summary>
			/// OAuth auth.
			/// </summary>
			Oauth,
			/// <summary>
			/// OpenID Connect (OIDC) Auth.
			/// </summary>
			OidcAuth,
		}
    }
}