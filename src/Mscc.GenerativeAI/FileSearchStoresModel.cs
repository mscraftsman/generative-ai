#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
#endif
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;

namespace Mscc.GenerativeAI
{
    public class FileSearchStoresModel : BaseModel
    {
        internal override string Version => ApiVersion.V1Beta;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSearchStoresModel"/> class.
        /// </summary>
        public FileSearchStoresModel() : this(httpClientFactory: null, logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSearchStoresModel"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public FileSearchStoresModel(IHttpClientFactory? httpClientFactory = null, ILogger? logger = null) : base(httpClientFactory, logger) { }

        /// <summary>
        /// Creates an empty <see cref="FileSearchStore"/>.
        /// </summary>
        /// <param name="request">Required. The `FileSearchStore`.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<FileSearchStore> Create(FileSearchStore request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            
            var url = "{BaseUrlGoogleAi}/fileSearchStores";
            return await PostAsync<FileSearchStore, FileSearchStore>(request, url, string.Empty, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }
        
        /// <summary>
        /// Deletes a <see cref="FileSearchStore"/>.
        /// </summary>
        /// <param name="fileSearchStoreName">Required. Immutable. The name of the `FileSearchStore` to import the file into. Example: `fileSearchStores/my-file-search-store-123`</param>
        /// <param name="force">Optional. If set to true, any `Chunk`s and objects related to this `Document` will also be deleted. If false (the default), a `FAILED_PRECONDITION` error will be returned if `Document` contains any `Chunk`s.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="fileSearchStoreName"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<string> Delete(string fileSearchStoreName,
            bool force,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(fileSearchStoreName)) throw new ArgumentException("Value cannot be null or empty.", nameof(fileSearchStoreName));
            fileSearchStoreName = fileSearchStoreName.SanitizeFileSearchStoreName();
            
            var url = $"{BaseUrlGoogleAi}/{fileSearchStoreName}";
            var queryStringParams = new Dictionary<string, string?>() { [nameof(force)] = Convert.ToString(force) };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
#if NET472_OR_GREATER || NETSTANDARD2_0
            return await response.Content.ReadAsStringAsync();
#else
            return await response.Content.ReadAsStringAsync(cancellationToken);
#endif
        }
        
        /// <summary>
        /// Gets information about a specific <see cref="FileSearchStore"/>.
        /// </summary>
        /// <param name="fileSearchStoreName">Required. Immutable. The name of the `FileSearchStore` to import the file into. Example: `fileSearchStores/my-file-search-store-123`</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="fileSearchStoreName"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<FileSearchStore> Get(string fileSearchStoreName,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(fileSearchStoreName)) throw new ArgumentException("Value cannot be null or empty.", nameof(fileSearchStoreName));
            fileSearchStoreName = fileSearchStoreName.SanitizeFileSearchStoreName();

            var url = $"{BaseUrlGoogleAi}/{fileSearchStoreName}";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<FileSearchStore>(response);
        }
        
        /// <summary>
        /// Lists all <see cref="FileSearchStore"/>s owned by the user.
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageToken"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<ListFileSearchStoresResponse> List(int? pageSize = 50,
            string? pageToken = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            var url = "{BaseUrlGoogleAi}/fileSearchStores";
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(pageSize)] = Convert.ToString(pageSize, CultureInfo.InvariantCulture),
                [nameof(pageToken)] = pageToken
            };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<ListFileSearchStoresResponse>(response);
        }

        /// <summary>
        /// Imports a `File` from File Service to a `FileSearchStore`.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="fileSearchStoreName">Required. Immutable. The name of the `FileSearchStore` to import the file into. Example: `fileSearchStores/my-file-search-store-123`</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="fileSearchStoreName"/> is <see langword="null"/> or empty.</exception>
        public async Task<Operation> ImportFile(ImportFileRequest request,
            string fileSearchStoreName,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(fileSearchStoreName)) throw new ArgumentException("Value cannot be null or empty", nameof(fileSearchStoreName));
            fileSearchStoreName = fileSearchStoreName.SanitizeFileSearchStoreName();
            
            var url = "{BaseUrlGoogleAi}/{fileSearchStoresName}:importFile";
            return await PostAsync<ImportFileRequest, Operation>(request, url, string.Empty, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }
    }
}