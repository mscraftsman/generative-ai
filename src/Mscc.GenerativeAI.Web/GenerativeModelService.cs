using System.Net.Http;
using Microsoft.Extensions.Options;

namespace Mscc.GenerativeAI.Web
{
    public interface IGenerativeModelService
    {
        GenerativeModel Model { get; }
        
        GenerativeModel CreateInstance();
        
        GenerativeModel CreateInstance(string model);
    }

    public sealed class GenerativeModelService : IGenerativeModelService
    {
        private readonly IGenerativeAI _generativeAi;
        private readonly GenerativeModel _model;

        public GenerativeModelService(IOptions<GenerativeAIOptions> options, IHttpClientFactory httpClientFactory)
        {
            var model = options?.Value?.Model ?? GenerativeAI.Model.Gemini25Pro;
            if (!string.IsNullOrEmpty(options?.Value.ProjectId))
            {
                _generativeAi = new VertexAI(projectId: options?.Value.ProjectId,
                    region: options?.Value.Region ?? options?.Value.Location,
                    httpClientFactory: httpClientFactory);
            }
            else
            {
                _generativeAi = new GoogleAI(apiKey: options?.Value.Credentials.ApiKey, httpClientFactory: httpClientFactory);
            }
            _model = _generativeAi.GenerativeModel(model: model);
        }

        public GenerativeModelService(IOptions<GenerativeAIOptions> options, string model, IHttpClientFactory httpClientFactory) : base()
        {
            if (!string.IsNullOrEmpty(options?.Value?.ProjectId))
            {
                _generativeAi = new VertexAI(projectId: options?.Value.ProjectId,
                    region: options?.Value.Region ?? options?.Value.Location,
                    httpClientFactory: httpClientFactory);
            }
            else
            {
                _generativeAi = new GoogleAI(apiKey: options?.Value.Credentials.ApiKey, httpClientFactory: httpClientFactory);
            }
            _model = _generativeAi.GenerativeModel(model: model);
        }
        
        /// <summary>
        /// Default instance of the model.
        /// </summary>
        public GenerativeModel Model => _model;
        
        /// <summary>
        /// Creates a new instance of the current model.
        /// </summary>
        /// <returns>A new instance of the current model.</returns>
        public GenerativeModel CreateInstance()
        {
            return _generativeAi.GenerativeModel(model: _model.Name);
        }

        /// <summary>
        /// Creates a new instance of the specified model.
        /// </summary>
        /// <param name="model">The model name to create.</param>
        /// <returns>A new instance of the model.</returns>
        public GenerativeModel CreateInstance(string model)
        {
            return _generativeAi.GenerativeModel(model: model);
        }
    }
}
