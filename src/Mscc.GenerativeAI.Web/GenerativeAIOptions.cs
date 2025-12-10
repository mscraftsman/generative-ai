namespace Mscc.GenerativeAI.Web
{
    /// <summary>
    /// Provides options for configuring the Generative AI service.
    /// </summary>
    public interface IGenerativeAIOptions
    {
        /// <summary>
        /// Gets the scheme used for authentication.
        /// </summary>
        string Scheme { get; }
        /// <summary>
        /// Gets or sets the credentials used to authenticate with the service.
        /// </summary>
        GenerativeAICredentials Credentials { get; set; }
        /// <summary>
        /// Gets or sets the project ID for the service.
        /// </summary>
        string ProjectId { get; set; }
        /// <summary>
        /// Gets or sets the region for the service.
        /// </summary>
        string Region { get; set; }
        /// <summary>
        /// Gets or sets the location for the service.
        /// </summary>
        string Location { get; set; }
        /// <summary>
        /// Gets or sets the model to use for the service.
        /// </summary>
        string Model { get; set; }
    }

    /// <summary>
    /// Provides options for configuring the Generative AI service.
    /// </summary>
    public sealed class GenerativeAIOptions : IGenerativeAIOptions
    {
        private string model;

        /// <summary>
        /// Gets the scheme used for authentication.
        /// </summary>
        public string Scheme { get; internal set; } = "x-goog-api-key";
        /// <summary>
        /// Gets or sets the credentials used to authenticate with the service.
        /// </summary>
        public GenerativeAICredentials? Credentials { get; set; } // = new GenerativeAICredentials();
        /// <summary>
        /// Gets or sets the project ID for the service.
        /// </summary>
        public string? ProjectId { get; set; } // = string.Empty;
        /// <summary>
        /// Gets or sets the region for the service.
        /// </summary>
        public string? Region { get; set; } // = string.Empty;
        /// <summary>
        /// Gets or sets the location for the service.
        /// </summary>
        public string? Location { get; set; } // = string.Empty;
        /// <summary>
        /// Gets or sets the access token for the service.
        /// </summary>
        public string? AccessToken { get; set; }
        /// <summary>
        /// Gets or sets the model to use for the service.
        /// </summary>
        public string? Model
        {
            get => !string.IsNullOrEmpty(model) ? model : GenerativeAI.Model.Gemini25Pro;
            set => model = value;
        }
    }
}