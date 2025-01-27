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
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
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

            var url = $"{BaseUrlVertexAi}/tuningJobs";
            url = ParseUrl(url);
            var json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = payload;
            var response = await SendAsync(httpRequest, cancellationToken);
            await response.EnsureSuccessAsync();
            return await Deserialize<TuningJob>(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<List<TuningJob>> List(CancellationToken cancellationToken = default)
        {
            var url = $"{BaseUrlVertexAi}/tuningJobs";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, cancellationToken);
            await response.EnsureSuccessAsync();
            var tuningJobs = await Deserialize<ListTuningJobResponse>(response);
            return tuningJobs.TuningJobs;
        }

        /// <summary>
        /// Gets metadata of a tuning job.
        /// </summary>
        /// <param name="tuningJobId">Required. The ID of the tuning job. Format: `tuningJobs/{id}`</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The details of a tuning job.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="tuningJobId"/> is <see langword="null"/> or empty.</exception>
        public async Task<TuningJob> Get(string tuningJobId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tuningJobId)) throw new ArgumentException("Value cannot be null or empty.", nameof(tuningJobId));

            tuningJobId = tuningJobId.SanitizeTuningJobsName();

            var url = $"{BaseUrlVertexAi}/{tuningJobId}";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, cancellationToken);
            await response.EnsureSuccessAsync();
            return await Deserialize<TuningJob>(response);
        }

        /// <summary>
        /// Cancels a tuning job.
        /// </summary>
        /// <param name="tuningJobId">Required. The ID of the tuning job. Format: `tuningJobs/{id}`</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>If successful, the response body is empty.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="tuningJobId"/> is <see langword="null"/> or empty.</exception>
        public async Task<string> Cancel(string tuningJobId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tuningJobId)) throw new ArgumentException("Value cannot be null or empty.", nameof(tuningJobId));

            tuningJobId = tuningJobId.SanitizeTuningJobsName();

            var method = "cancel";
            var url = $"{BaseUrlVertexAi}/{tuningJobId}:{method}";
            url = ParseUrl(url, method);
            var payload = new StringContent(string.Empty, Encoding.UTF8, Constants.MediaType);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = payload;
            var response = await SendAsync(httpRequest, cancellationToken);
            await response.EnsureSuccessAsync();
#if NET472_OR_GREATER || NETSTANDARD2_0
            return await response.Content.ReadAsStringAsync();
#else
            return await response.Content.ReadAsStringAsync(cancellationToken);
#endif
        }

        /// <summary>
        /// Deletes a tuning job.
        /// </summary>
        /// <param name="tuningJobId">Required. The ID of the tuning job. Format: `tuningJobs/{id}`</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>If successful, the response body is empty.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="tuningJobId"/> is <see langword="null"/> or empty.</exception>
        public async Task<string> Delete(string tuningJobId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tuningJobId)) throw new ArgumentException("Value cannot be null or empty.", nameof(tuningJobId));

            tuningJobId = tuningJobId.SanitizeTuningJobsName();

            var url = $"{BaseUrlVertexAi}/{tuningJobId}";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, url);
            var response = await SendAsync(httpRequest, cancellationToken);
            await response.EnsureSuccessAsync();
#if NET472_OR_GREATER || NETSTANDARD2_0
            return await response.Content.ReadAsStringAsync();
#else
            return await response.Content.ReadAsStringAsync(cancellationToken);
#endif
        }
    }
}