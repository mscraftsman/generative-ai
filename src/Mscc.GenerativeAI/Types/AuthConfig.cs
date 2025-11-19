namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Auth configuration to run the extension.
    /// </summary>
    public class AuthConfig
    {
        /// <summary>
        /// Config for API key auth.
        /// </summary>
        public ApiKeyConfig? ApiKeyConfig { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public AuthType? AuthType { get; set; }
        /// <summary>
        /// Config for Google Service Account auth.
        /// </summary>
        public AuthConfigGoogleServiceAccountConfig? GoogleServiceAccountConfig { get; set; }
        /// <summary>
        /// Config for HTTP basic auth.
        /// </summary>
        public AuthConfigHttpBasicAuthConfig? HttpBasicAuthConfig { get; set; }
        /// <summary>
        /// Config for user OAuth auth.
        /// </summary>
        public AuthConfigOauthConfig? OauthConfig { get; set; }
        /// <summary>
        /// Config for OpenID Connect auth.
        /// </summary>
        public AuthConfigOidcConfig? OidcConfig { get; set; }
    }
}