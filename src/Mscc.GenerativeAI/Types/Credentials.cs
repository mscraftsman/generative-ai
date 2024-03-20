namespace Mscc.GenerativeAI
{
    internal class Credentials : ClientSecrets
    {
        private string _projectId;
        
        public ClientSecrets Web { get; set; }
        public ClientSecrets Installed { get; set; }

        public string Account { get; set; }
        public string RefreshToken { get; set; }
        public string Type { get; set; }
        public string UniverseDomain { get; set; }

        public string ProjectId
        {
            get => _projectId;
            set => _projectId = value;
        }
        
        public virtual string QuotaProjectId
        {
            get => _projectId;
            set => _projectId = value;
        }
    }

    internal class ClientSecrets
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string[] RedirectUris { get; set; }
        public string AuthUri { get; set; }
        public string AuthProviderX509CertUrl { get; set; }
        public string TokenUri { get; set; }
    }
}