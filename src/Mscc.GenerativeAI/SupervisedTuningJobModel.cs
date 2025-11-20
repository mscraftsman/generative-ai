using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI.Types;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SupervisedTuningJobModel : BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SupervisedTuningJobModel"/> class.
        /// </summary>
        public SupervisedTuningJobModel() : this(httpClientFactory: null, logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SupervisedTuningJobModel"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public SupervisedTuningJobModel(IHttpClientFactory? httpClientFactory = null, ILogger? logger = null) : base(httpClientFactory, logger) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SupervisedTuningJobModel"/> class with access to Vertex AI Gemini API.
        /// </summary>
        /// <param name="projectId">Identifier of the Google Cloud project</param>
        /// <param name="region">Region to use</param>
        /// <param name="accessToken">Access token for the Google Cloud project.</param>
        /// <param name="model">Model to use</param>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public SupervisedTuningJobModel(string? projectId = null, 
            string? region = null,
	        string? accessToken = null,
            string? model = null, 
            IHttpClientFactory? httpClientFactory = null, 
            ILogger? logger = null) : base(projectId, region, model, accessToken, httpClientFactory, logger) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<TuningJob> Create(CreateTuningJobRequest request,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = $"{BaseUrlVertexAi}/tuningJobs";
            return await PostAsync<CreateTuningJobRequest, TuningJob>(request, url, string.Empty, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<List<TuningJob>> List(RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            var url = $"{BaseUrlVertexAi}/tuningJobs";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            var tuningJobs = await Deserialize<ListTuningJobResponse>(response);
            return tuningJobs.TuningJobs;
        }

        /// <summary>
        /// Gets metadata of a tuning job.
        /// </summary>
        /// <param name="tuningJobId">Required. The ID of the tuning job. Format: `tuningJobs/{id}`</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The details of a tuning job.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="tuningJobId"/> is <see langword="null"/> or empty.</exception>
        public async Task<TuningJob> Get(string tuningJobId,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tuningJobId)) throw new ArgumentException("Value cannot be null or empty.", nameof(tuningJobId));

            tuningJobId = tuningJobId.SanitizeTuningJobsName();

            var url = $"{BaseUrlVertexAi}/{tuningJobId}";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<TuningJob>(response);
        }

        /// <summary>
        /// Cancels a tuning job.
        /// </summary>
        /// <param name="tuningJobId">Required. The ID of the tuning job. Format: `tuningJobs/{id}`</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>If successful, the response body is empty.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="tuningJobId"/> is <see langword="null"/> or empty.</exception>
        public async Task<string> Cancel(string tuningJobId,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tuningJobId)) throw new ArgumentException("Value cannot be null or empty.", nameof(tuningJobId));

            tuningJobId = tuningJobId.SanitizeTuningJobsName();

            var method = "cancel";
            var url = $"{BaseUrlVertexAi}/{tuningJobId}:{method}";
            return await PostAsync<string, string>(string.Empty, url, method, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Deletes a tuning job.
        /// </summary>
        /// <param name="tuningJobId">Required. The ID of the tuning job. Format: `tuningJobs/{id}`</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>If successful, the response body is empty.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="tuningJobId"/> is <see langword="null"/> or empty.</exception>
        public async Task<string> Delete(string tuningJobId,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tuningJobId)) throw new ArgumentException("Value cannot be null or empty.", nameof(tuningJobId));

            tuningJobId = tuningJobId.SanitizeTuningJobsName();

            var url = $"{BaseUrlVertexAi}/{tuningJobId}";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
#if NET472_OR_GREATER || NETSTANDARD2_0
            return await response.Content.ReadAsStringAsync();
#else
            return await response.Content.ReadAsStringAsync(cancellationToken);
#endif
        }
    }
}