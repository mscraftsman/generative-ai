namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Represents the credentials used to authenticate with the API.
    /// It de/serializes the content of the client_secret.json file for OAuth 2.0
    /// using either Desktop or Web approach, and supports Service Accounts on Google Cloud Platform.
    /// </summary>
    public sealed class Credentials : ClientSecrets
    {
        private string _projectId;
        
        /// <summary>
        /// Client secrets for web applications.
        /// </summary>
        public ClientSecrets Web { get; set; }
        /// <summary>
        /// Client secrets for desktop applications.
        /// </summary>
        public ClientSecrets Installed { get; set; }

        /// <summary>
        /// Account used in Google CLoud Platform.
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// Refresh token for the API to retrieve a new access token.
        /// </summary>
        public string RefreshToken { get; set; }
        /// <summary>
        /// Type of account in Google Cloud Platform.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Uri of domain
        /// </summary>
        public string UniverseDomain { get; set; }

        /// <summary>
        /// Project ID in Google Cloud Platform.
        /// </summary>
        public string ProjectId
        {
            get => _projectId;
            set => _projectId = value;
        }
        
        /// <summary>
        /// Project ID (quota) in Google Cloud Platform.
        /// </summary>
        public string QuotaProjectId
        {
            get => _projectId;
            set => _projectId = value;
        }
    }

    /// <summary>
    /// Represents the content of a client_secret.json file used in Google Cloud Platform
    /// to authenticate a user or service account.
    /// </summary>
    public partial class ClientSecrets
    {
        /// <summary>
        /// Client ID
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Client secret
        /// </summary>
        public string ClientSecret { get; set; }
        /// <summary>
        /// List of Callback URLs in case of a web application.
        /// </summary>
        public string[] RedirectUris { get; set; }
        /// <summary>
        /// Authentication endpoint.
        /// </summary>
        public string AuthUri { get; set; }
        /// <summary>
        /// URL to an X509 certificate provider.
        /// </summary>
        public string AuthProviderX509CertUrl { get; set; }
        /// <summary>
        /// Uri of token.
        /// </summary>
        public string TokenUri { get; set; }
    }
}