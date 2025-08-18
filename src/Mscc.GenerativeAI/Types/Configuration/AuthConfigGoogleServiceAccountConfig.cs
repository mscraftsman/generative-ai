namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Config for Google Service Account Authentication.
    /// </summary>
    public class AuthConfigGoogleServiceAccountConfig
    {
        /// <summary>
        /// Optional. The service account that the extension execution service runs as.
        /// </summary>
        /// <remarks>
        /// - If the service account is specified, the `iam.serviceAccounts.getAccessToken` permission should be
        /// granted to Vertex AI Extension Service Agent (https://cloud.google.com/vertex-ai/docs/general/access-control#service-agents)
        /// on the specified service account.
        /// - If not specified, the Vertex AI Extension Service Agent will be used to execute the Extension.
        /// </remarks>
        public string? ServiceAccount { get; set; }
    }
}