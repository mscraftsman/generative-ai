namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Config for OpenID Connect auth.
    /// </summary>
    public class AuthConfigOidcConfig
    {
        /// <summary>
        /// OpenID Connect formatted ID token for extension endpoint. Only used to propagate token from [[ExecuteExtensionRequest.runtime_auth_config]] at request time.
        /// </summary>
        public string? IdToken { get; set; }
        /// <summary>
        /// The service account used to generate an OpenID Connect (OIDC)-compatible JWT token signed by the Google OIDC Provider (accounts.google.com) for extension endpoint (https://cloud.google.com/iam/docs/create-short-lived-credentials-direct#sa-credentials-oidc). - The audience for the token will be set to the URL in the server url defined in the OpenApi spec. - If the service account is provided, the service account should grant `iam.serviceAccounts.getOpenIdToken` permission to Vertex AI Extension Service Agent (https://cloud.google.com/vertex-ai/docs/general/access-control#service-agents).
        /// </summary>
        public string? ServiceAccount { get; set; }
    }
}