namespace Mscc.GenerativeAI.Web
{
    public interface IGenerativeAIOptions
    {
        string Scheme { get; }
        GenerativeAICredentials Credentials { get; set; }
        string ProjectId { get; set; }
        string Region { get; set; }
        string Model { get; set; }
    }

    public sealed class GenerativeAIOptions : IGenerativeAIOptions
    {
        private string model;

        public string Scheme { get; internal set; } = "x-goog-api-key";
        public GenerativeAICredentials? Credentials { get; set; } // = new GenerativeAICredentials();
        public string? ProjectId { get; set; } // = string.Empty;
        public string? Region { get; set; } // = string.Empty;
        public string? Model
        {
            get => !string.IsNullOrEmpty(model) ? model : GenerativeAI.Model.Gemini25Pro;
            set => model = value;
        }
    }
}