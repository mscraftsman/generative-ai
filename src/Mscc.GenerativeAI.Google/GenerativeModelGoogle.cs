#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
#endif
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using System.Security.Cryptography.X509Certificates;

namespace Mscc.GenerativeAI.Google
{
    /// <summary>
    /// Helper class leveraging Google API Client Libraries.
    /// Auth 2.0 credential for accessing protected resources using an access token, as well as optionally refreshing 
    /// the access token when it expires using a refresh token.
    /// </summary>
    public class GenerativeModelGoogle
    {
        private readonly List<string> _scopes =
            new List<string> { "https://www.googleapis.com/auth/generative-language.retriever" };

        private string _clientFile = "client_secrets.json";
        private string _tokenFile = "tokens.json";
        private string _certificateFile = "key.p12";
        private string _certificatePassphrase;

        private ICredential _credential;
        private string ClientId;
        private string ClientSecret;

        public string ProjectId { get; set; }
        public string Region { get; set; }
        private List<SafetySetting> SafetySettings { get; set; }
        private GenerationConfig GenerationConfig { get; set; }
        private List<Tool> Tools { get; set; }

        /// <summary>
        /// Constructor via user credentials using client ID and secret.
        /// </summary>
        public GenerativeModelGoogle()
        {
            var clientSecrets = getClientSecrets();
            _credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                clientSecrets,
                _scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(_tokenFile)).Result;
        }

        /// <summary>
        /// Constructor via service account using its email address.
        /// </summary>
        /// <param name="serviceAccountEmail"></param>
        /// <param name="certificate"></param>
        /// <param name="passphrase"></param>
        private GenerativeModelGoogle(string serviceAccountEmail, string certificate = null, string passphrase = null)
        {
            var x509Certificate = new X509Certificate2(
                certificate ?? _certificateFile,
                passphrase ?? _certificatePassphrase,
                X509KeyStorageFlags.Exportable);
            _credential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(serviceAccountEmail) { Scopes = _scopes }.FromCertificate(
                    x509Certificate));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceAccountEmail"></param>
        /// <param name="certificate"></param>
        /// <param name="passphrase"></param>
        /// <returns></returns>
        public static GenerativeModelGoogle CreateInstance(string serviceAccountEmail = null, string certificate = null,
            string passphrase = null)
        {
            return serviceAccountEmail == null
                ? new GenerativeModelGoogle()
                : new GenerativeModelGoogle(serviceAccountEmail, certificate, passphrase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public GenerativeModel CreateModel(string model = Model.Gemini10Pro)
        {
            if (ProjectId == null) throw new ArgumentNullException(nameof(ProjectId));
            if (Region == null) throw new ArgumentNullException(nameof(Region));

            var accessToken = _credential.GetAccessTokenForRequestAsync().Result;
            var vertex = new VertexAI(ProjectId, Region);
            var generativeModel = vertex.GenerativeModel(model, GenerationConfig, SafetySettings);
            generativeModel.AccessToken = accessToken;
            return generativeModel;
        }

        private ClientSecrets getClientSecrets()
        {
            // _credentials = GoogleCredential.GetApplicationDefaultAsync();

            // if (File.Exists(_tokenFile))
            // {
            //     _credentials = await GoogleCredential.FromFileAsync(_tokenFile);
            // }
            // if (!_credentials.)
            ClientSecrets clientSecrets = null;

            if (!string.IsNullOrEmpty(ClientId))
            {
                clientSecrets = new ClientSecrets { ClientId = ClientId, ClientSecret = ClientSecret };
            }

            if (File.Exists(_clientFile))
            {
                using (var stream = new FileStream(_clientFile, FileMode.Open, FileAccess.Read))
                {
                    // clientSecrets = GoogleClientSecrets.Load(stream).Secrets;
                    clientSecrets = GoogleClientSecrets.FromStreamAsync(stream).Result.Secrets;
                }
            }

            return clientSecrets;
        }
    }
}