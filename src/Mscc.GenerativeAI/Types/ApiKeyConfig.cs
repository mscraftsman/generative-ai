namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Config for authentication with API key.
    /// </summary>
    public class ApiKeyConfig
    {
        /// <summary>
        /// Optional. The parameter name of the API key.
        /// E.g. If the API request is "https://example.com/act?api_key=", "api_key" would be the parameter name.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Optional. The name of the SecretManager secret version resource storing the API key.
        /// Format: `projects/{project}/secrets/{secrete}/versions/{version}` - If both `api_key_secret` and `api_key_string` are specified, this field takes precedence over `api_key_string`. - If specified, the `secretmanager.versions.access` permission should be granted to Vertex AI Extension Service Agent (https://cloud.google.com/vertex-ai/docs/general/access-control#service-agents) on the specified resource.
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
        /// Required. The SecretManager secret version resource name storing API key.
        /// e.g. projects/{project}/secrets/{secret}/versions/{version}
        /// </summary>
        private string? ApiKeySecretVersion { get; set; }   // different ApiKeyConfig
    }
}