namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Config for user oauth.
    /// </summary>
    public class AuthConfigOauthConfig
    {
        /// <summary>
        /// Access token for extension endpoint. Only used to propagate token from [[ExecuteExtensionRequest.runtime_auth_config]] at request time.
        /// </summary>
        public string? AccessToken { get; set; }
        /// <summary>
        /// The service account used to generate access tokens for executing the Extension. - If the service account is specified, the `iam.serviceAccounts.getAccessToken` permission should be granted to Vertex AI Extension Service Agent (https://cloud.google.com/vertex-ai/docs/general/access-control#service-agents) on the provided service account.
        /// </summary>
        public string? ServiceAccount { get; set; }
    }
}