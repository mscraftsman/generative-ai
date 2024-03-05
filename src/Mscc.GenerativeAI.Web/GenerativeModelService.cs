using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mscc.GenerativeAI.Web
{
    public interface IGenerativeModelService
    {
        Task<CountTokensResponse> CountTokens(GenerateContentRequest? request);
        Task<CountTokensResponse> CountTokens(string? prompt);
        Task<GenerateContentResponse> GenerateContent(GenerateContentRequest prompt);
        Task<GenerateContentResponse> GenerateContent(string prompt);
        Task<ModelResponse> GetModel(string model = Model.GeminiPro);
        Task<List<ModelResponse>> ListModels();
    }

    public class GenerativeModelService : IGenerativeModelService
    {
        private readonly GenerativeModel model;

        public GenerativeModelService(IOptions<GenerativeAIOptions> options)
        {
            var model = options?.Value?.Model ?? Model.Gemini10Pro;
            if (!string.IsNullOrEmpty(options?.Value.ProjectId))
            {
                var vertex = new VertexAI(options?.Value.ProjectId, options?.Value.Region);
                this.model = vertex.GenerativeModel(model: model);
            }
            else
            {
                this.model = new GenerativeModel(apiKey: options?.Value.Credentials.ApiKey, model: model);
            }
        }

        public GenerativeModelService(IOptions<GenerativeAIOptions> options, string model) : base()
        {
            if (!string.IsNullOrEmpty(options?.Value?.ProjectId))
            {
                var vertex = new VertexAI(options?.Value.ProjectId, options?.Value.Region);
                this.model = vertex.GenerativeModel(model: model);
            }
            else
            {
                this.model = new GenerativeModel(apiKey: options?.Value.Credentials.ApiKey, model: model);
            }
        }

        public async Task<List<ModelResponse>> ListModels()
        {
            return await model.ListModels();
        }

        public async Task<ModelResponse> GetModel(string model = Model.GeminiPro)
        {
            return await this.model.GetModel(model);
        }

        public async Task<GenerateContentResponse> GenerateContent(GenerateContentRequest prompt)
        {
            return await model.GenerateContent(prompt);
        }

        public async Task<GenerateContentResponse> GenerateContent(string prompt)
        {
            return await model.GenerateContent(prompt);
        }

        public async Task<CountTokensResponse> CountTokens(GenerateContentRequest? request)
        {
            return await model.CountTokens(request);
        }

        public async Task<CountTokensResponse> CountTokens(string? prompt)
        {
            return await model.CountTokens(prompt);
        }
    }
}
