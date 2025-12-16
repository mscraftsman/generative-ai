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

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// JiraQueries contains the Jira queries and corresponding authentication.
	/// </summary>
	public partial class JiraSourceJiraQueries
	{
		/// <summary>
		/// Required. The SecretManager secret version resource name (e.g. projects/{project}/secrets/{secret}/versions/{version}) storing the Jira API key. See [Manage API tokens for your Atlassian account](https://support.atlassian.com/atlassian-account/docs/manage-api-tokens-for-your-atlassian-account/).
		/// </summary>
		public ApiAuthApiKeyConfig? ApiKeyConfig { get; set; }
		/// <summary>
		/// A list of custom Jira queries to import. For information about JQL (Jira Query Language), see https://support.atlassian.com/jira-service-management-cloud/docs/use-advanced-search-with-jira-query-language-jql/
		/// </summary>
		public List<string>? CustomQueries { get; set; }
		/// <summary>
		/// Required. The Jira email address.
		/// </summary>
		public string? Email { get; set; }
		/// <summary>
		/// A list of Jira projects to import in their entirety.
		/// </summary>
		public List<string>? Projects { get; set; }
		/// <summary>
		/// Required. The Jira server URI.
		/// </summary>
		public string? ServerUri { get; set; }
    }
}