namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Config for HTTP Basic Authentication.
    /// </summary>
    public class AuthConfigHttpBasicAuthConfig
    {
        /// <summary>
        /// Required. The name of the SecretManager secret version resource storing the base64 encoded credentials.
        /// </summary>
        /// <remarks>
        /// Format: `projects/{project}/secrets/{secrete}/versions/{version}`
        /// - If specified, the `secretmanager.versions.access` permission should be granted to Vertex AI Extension
        /// Service Agent (https://cloud.google.com/vertex-ai/docs/general/access-control#service-agents) on the specified resource.
        /// </remarks>
        public string CredentialSecret { get; set; }
    }
}