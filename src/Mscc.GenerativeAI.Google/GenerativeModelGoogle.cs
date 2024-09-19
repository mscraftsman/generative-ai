#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
#endif
using gauth = Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using System.Security.Cryptography.X509Certificates;

namespace Mscc.GenerativeAI.Google
{
    /// <summary>
    /// Helper class leveraging Google API Client Libraries.
    /// OAuth 2.0 credential for accessing protected resources using an access token, as well as optionally refreshing 
    /// the access token when it expires using a refresh token.
    /// </summary>
    // Reference: https://cloud.google.com/docs/authentication 
    public class GenerativeModelGoogle
    {
        private readonly List<string> _scopes =
        [
            "https://www.googleapis.com/auth/cloud-platform",
            "https://www.googleapis.com/auth/generative-language.retriever",
            "https://www.googleapis.com/auth/generative-language.tuning"
        ];

        private string _clientFile = "client_secret.json";
        private string _tokenFile = "token.json";
        private string _certificateFile = "key.p12";
        private string _certificatePassphrase;

        private gauth.ICredential _credential;
        // private string ClientId;
        // private string ClientSecret;

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
            _credential = gauth.GoogleWebAuthorizationBroker.AuthorizeAsync(
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
        private GenerativeModelGoogle(string serviceAccountEmail, string? certificate = null, string? passphrase = null)
        {
            var x509Certificate = new X509Certificate2(
                certificate ?? _certificateFile,
                passphrase ?? _certificatePassphrase,
                X509KeyStorageFlags.Exportable);
            _credential = new gauth.ServiceAccountCredential(
                new gauth.ServiceAccountCredential.Initializer(serviceAccountEmail)
                {
                    Scopes = _scopes
                }.FromCertificate(x509Certificate));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceAccountEmail"></param>
        /// <param name="certificate"></param>
        /// <param name="passphrase"></param>
        /// <returns></returns>
        public static GenerativeModelGoogle CreateInstance(string? serviceAccountEmail = null, 
            string? certificate = null,
            string? passphrase = null)
        {
            return serviceAccountEmail == null
                ? new GenerativeModelGoogle()
                : new GenerativeModelGoogle(serviceAccountEmail, certificate, passphrase);
        }

        /// <summary>
        /// Returns an instance of the specified model.
        /// </summary>
        /// <param name="model">The model name, ie. "gemini-pro"</param>
        /// <returns>The model.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public GenerativeModel CreateModel(string model = Model.Gemini15Pro)
        {
            if (ProjectId == null) throw new ArgumentNullException(nameof(ProjectId));
            if (Region == null) throw new ArgumentNullException(nameof(Region));

            var accessToken = _credential.GetAccessTokenForRequestAsync().Result;   // _credential.Token.AccessToken
            var vertex = new VertexAI(ProjectId, Region);
            var generativeModel = vertex.GenerativeModel(model, GenerationConfig, SafetySettings);
            generativeModel.AccessToken = accessToken;
            return generativeModel;
        }

        private gauth.ClientSecrets getClientSecrets()
        {
            // _credentials = GoogleCredential.GetApplicationDefaultAsync();

            // if (File.Exists(_tokenFile))
            // {
            //     _credentials = await GoogleCredential.FromFileAsync(_tokenFile);
            // }
            // if (!_credentials.)
            gauth.ClientSecrets clientSecrets = null;

            // if (!string.IsNullOrEmpty(ClientId))
            // {
            //     clientSecrets = new ClientSecrets { ClientId = ClientId, ClientSecret = ClientSecret };
            // }

            if (File.Exists(_clientFile))
            {
                using (var stream = new FileStream(_clientFile, FileMode.Open, FileAccess.Read))
                {
                    // clientSecrets = GoogleClientSecrets.Load(stream).Secrets;
                    clientSecrets = gauth.GoogleClientSecrets.FromStreamAsync(stream).Result.Secrets;
                }
            }

            return clientSecrets;
        }

        // private ICredential LoadCredentials()
        // {
        //     ICredential credentials = null;
        //     if (File.Exists(_tokenFile))
        //     {
        //         using (var stream = new FileStream(_tokenFile, FileMode.Open, FileAccess.Read))
        //         {
        //             credentials = GoogleCredential.FromStreamAsync(stream, new CancellationToken()).Result
        //                 .UnderlyingCredential;
        //         }
        //     }
        //
        //     if (credentials == null || credentials.)
        //     {
        //         var clientSecrets = getClientSecrets();
        //         var flow = new GoogleAuthorizationCodeFlow(
        //             new GoogleAuthorizationCodeFlow.Initializer()
        //             {
        //                 ClientSecrets = clientSecrets,
        //             });
        //         flow.
        //     }
        //
        //     return credentials;
        // }
    }
}