#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endif
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SupervisedTuningJobModel : BaseModel
    {
        private const string UrlVertexAi = "projects/{projectId}/locations/{region}/tuningJobs";
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SupervisedTuningJobModel"/> class.
        /// </summary>
        public SupervisedTuningJobModel() : this(logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SupervisedTuningJobModel"/> class.
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public SupervisedTuningJobModel(ILogger? logger = null) : base(logger) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SupervisedTuningJobModel"/> class with access to Vertex AI Gemini API.
        /// </summary>
        /// <param name="projectId">Identifier of the Google Cloud project</param>
        /// <param name="region">Region to use</param>
        /// <param name="model">Model to use</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public SupervisedTuningJobModel(string? projectId = null, string? region = null,
            string? model = null, ILogger? logger = null) : base(projectId, region, model, logger) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<TuningJob> Create(CreateTuningJobRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (!(request.BaseModel.Equals($"{GenerativeAI.Model.Gemini15Pro002}", StringComparison.InvariantCultureIgnoreCase) ||
                  request.BaseModel.Equals($"{GenerativeAI.Model.Gemini15Flash002}", StringComparison.InvariantCultureIgnoreCase) ||
                  request.BaseModel.Equals($"{GenerativeAI.Model.Gemini10Pro002}", StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new NotSupportedException();
            }

            var url = $"{BaseUrlVertexAi}/{UrlVertexAi}";
            url = ParseUrl(url);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            var response = await Client.PostAsync(url, payload, cancellationToken);
            await response.EnsureSuccessAsync();
            return await Deserialize<TuningJob>(response);
        }

        public async Task<List<TuningJob>> List()
        {
            var url = $"{BaseUrlVertexAi}/{UrlVertexAi}";
            url = ParseUrl(url);
            var response = await Client.GetAsync(url);
            await response.EnsureSuccessAsync();
            var tuningJobs = await Deserialize<ListTuningJobResponse>(response);
            return tuningJobs.TuningJobs;
        }

        public async Task<TuningJob> Get(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

            // name = name.SanitizeCachedContentName();

            var url = $"{BaseUrlVertexAi}/{name}";
            url = ParseUrl(url);
            var response = await Client.GetAsync(url);
            await response.EnsureSuccessAsync();
            return await Deserialize<TuningJob>(response);
        }

        /// <summary>
        /// Cancels a tuning job.
        /// </summary>
        /// <param name="name">Required. The resource name referring to the content cache entry. Format: `cachedContents/{id}`</param>
        /// <returns>If successful, the response body is empty.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="name"/> is <see langword="null"/> or empty.</exception>
        public async Task<string> Cancel(string name,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

            // name = name.SanitizeCachedContentName();

            var method = "cancel";
            var url = $"{BaseUrlVertexAi}/{name}:{method}";
            url = ParseUrl(url, method);
            var payload = new StringContent(string.Empty, Encoding.UTF8, Constants.MediaType);
            var response = await Client.PostAsync(url, payload, cancellationToken);
            await response.EnsureSuccessAsync();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Deletes a tuning job.
        /// </summary>
        /// <param name="name">Required. The resource name referring to the content cache entry. Format: `cachedContents/{id}`</param>
        /// <returns>If successful, the response body is empty.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="name"/> is <see langword="null"/> or empty.</exception>
        public async Task<string> Delete(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

            // name = name.SanitizeCachedContentName();

            var url = $"{BaseUrlVertexAi}/{name}";
            url = ParseUrl(url);
            var response = await Client.DeleteAsync(url);
            await response.EnsureSuccessAsync();
            return await response.Content.ReadAsStringAsync();
        }
    }
}