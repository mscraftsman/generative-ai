namespace Mscc.GenerativeAI.Web
{
    public interface IGenerativeAIOptions
    {
        string Scheme { get; }
        GenerativeAICredentials Credentials { get; set; }
        string ProjectId { get; set; }
        string Region { get; set; }
    }

    public class GenerativeAIOptions : IGenerativeAIOptions
    {
        public string Scheme { get; internal set; }

        public GenerativeAIOptions()
        {
            Scheme = "x-goog-api-key";
        }

        public GenerativeAICredentials Credentials { get; set; } = new GenerativeAICredentials();
        public string ProjectId { get; set; } = default;
        public string Region { get; set; } = default;
    }
}