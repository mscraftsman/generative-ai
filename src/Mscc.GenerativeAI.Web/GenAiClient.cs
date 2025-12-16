using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mscc.GenerativeAI.Types;
using System.Net.Http;

namespace Mscc.GenerativeAI.Web
{
    public interface IGeniAiClient
    {
        GenerativeModel Models { get; }
        FileSearchStoresModel FileSearchStores { get; }
        FileSearchStoresModel Stores { get; }
        OperationsModel Operations { get; }
    }
    
    public sealed class GenAiClient : IGeniAiClient
    {
        private IOptions<GenerativeAIOptions> options;
        private IHttpClientFactory httpClientFactory;
        private ILogger<GenAiClient> logger;
        private readonly IGenerativeAI _generativeAi;
        private string _model;

        public GenerativeModel Models => _generativeAi.GenerativeModel(model: _model, logger: logger);
        public FileSearchStoresModel FileSearchStores => _generativeAi.FileSearchStoresModel();
        public FileSearchStoresModel Stores => FileSearchStores;
        public OperationsModel Operations => _generativeAi.OperationsModel();

        public GenAiClient(IOptions<GenerativeAIOptions> options, 
            RequestOptions? requestOptions,
            IHttpClientFactory httpClientFactory, 
            ILogger<GenAiClient> logger)
        {
            this.options = options;
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;

            _model = options?.Value?.Model ?? Model.Gemini25Pro;
            if (!string.IsNullOrEmpty(options?.Value.ProjectId))
            {
                _generativeAi = new VertexAI(projectId: options?.Value.ProjectId,
                    region: options?.Value.Region ?? options?.Value.Location,
                    httpClientFactory: httpClientFactory,
                    requestOptions: requestOptions,
                    logger: logger);
            }
            else
            {
                _generativeAi = new GoogleAI(apiKey: options?.Value.Credentials?.ApiKey, 
                    httpClientFactory: httpClientFactory,
                    requestOptions: requestOptions,
                    logger: logger);
            }
        }
    }
}