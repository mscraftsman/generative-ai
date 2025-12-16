using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI.Types;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace Mscc.GenerativeAI
{
    public class FileSearchStoresModel : BaseModel
    {
        private readonly DocumentsModel? _documents;
        internal override string Version => ApiVersion.V1Beta;
        
        public DocumentsModel Documents
        {
            get
            {
                var result = _documents ?? new DocumentsModel();
                result.ApiKey = _apiKey;
                result.AccessToken = _apiKey is null ? _accessToken : null;
                return result;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSearchStoresModel"/> class.
        /// </summary>
        public FileSearchStoresModel() : this(httpClientFactory: null, logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSearchStoresModel"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public FileSearchStoresModel(IHttpClientFactory? httpClientFactory = null,
            ILogger? logger = null) : base(httpClientFactory, logger)
        {
            _documents = new DocumentsModel(httpClientFactory, logger);
        }

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
        public async Task<FileSearchStore> Create(FileSearchStore? request = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            request ??= new FileSearchStore();
            
            var url = "{BaseUrlGoogleAi}/fileSearchStores";
            return await PostAsync<FileSearchStore, FileSearchStore>(request, url, string.Empty, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        public async Task<FileSearchStore> Create(string displayName,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            var request = new FileSearchStore
            {
                DisplayName = displayName
            };
            return await Create(request, requestOptions, cancellationToken);
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
            bool force = false,
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
        /// <param name="pageSize">The maximum number of items to return (per page).</param>
        /// <param name="pageToken">A page token, received from a previous List call. Provide the pageToken returned by one request as an argument to the next request to retrieve the next page.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<ListFileSearchStoresResponse> List(int? pageSize = 10,
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
        /// Imports a <see cref="FileResource"/> from File Service to a <see cref="FileSearchStore"/>.
        /// </summary>
        /// <param name="name">Required. Immutable. The name of the <see cref="FileSearchStore"/> to import the file into. Example: `fileSearchStores/my-file-search-store-123`</param>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="name"/> is <see langword="null"/> or empty.</exception>
        public async Task<Operation> ImportFile(string name,
            ImportFileRequest request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty", nameof(name));
            name = name.SanitizeFileSearchStoreName();
            
            var url = $"{BaseUrlGoogleAi}/{name}:importFile";
            return await PostAsync<ImportFileRequest, Operation>(request, url, string.Empty, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }


        /// <summary>
        /// Imports a <see cref="FileResource"/> from File Service to a <see cref="FileSearchStore"/>.
        /// </summary>
        /// <param name="name">Required. Immutable. The name of the <see cref="FileSearchStore"/> to import the file into. Example: `fileSearchStores/my-file-search-store-123`</param>
        /// <param name="fileResource">The <see cref="FileResource"/> from the Files API.</param>
        /// <param name="customMetadata">Custom metadata to be associated with the file.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="name"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="fileResource"/> is <see langword="null"/>.</exception>
        public async Task<Operation> ImportFile(string name,
            FileResource fileResource,
            List<CustomMetadata>? customMetadata = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (fileResource is null) throw new ArgumentNullException(nameof(fileResource));
            var request = new ImportFileRequest()
            {
                FileName = fileResource.Name,
                CustomMetadata = customMetadata
            };
            return await ImportFile(name, request, requestOptions, cancellationToken);
        }


        /// <summary>
        /// Imports a <see cref="FileResource"/> from File Service to a <see cref="FileSearchStore"/>.
        /// </summary>
        /// <param name="name">Required. Immutable. The name of the <see cref="FileSearchStore"/> to import the file into. Example: `fileSearchStores/my-file-search-store-123`</param>
        /// <param name="filename">Name of the <see cref="FileResource"/>.</param>
        /// <param name="customMetadata">Custom metadata to be associated with the file.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An operation of the imported file.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="name"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="filename"/> is <see langword="null"/> or empty.</exception>
        public async Task<Operation> ImportFile(string name,
            string filename,
            List<CustomMetadata>? customMetadata = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentException("Value cannot be null or empty", nameof(name));
            var request = new ImportFileRequest()
            {
                FileName = filename,
                CustomMetadata = customMetadata
            };
            return await ImportFile(name, request, requestOptions, cancellationToken);
        }

        /// <summary>
        /// Uploads data to a <see cref="FileSearchStore"/>, preprocesses and chunks before storing it in a <see cref="FileSearchStore"/> Document.
        /// </summary>
        /// <param name="name">Name of the File Search Store.</param>
        /// <param name="file">URI or path to the file to upload.</param>
        /// <param name="displayName">A name displayed for the uploaded file.</param>
        /// <param name="config">Configuration settings for the uploaded file.</param>
        /// <param name="resumable">Flag indicating whether to use resumable upload.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An operation of the uploaded file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="file"/> is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the file <paramref name="file"/> is not found.</exception>
        /// <exception cref="MaxUploadFileSizeException">Thrown when the file size exceeds the maximum allowed size.</exception>
        /// <exception cref="UploadFileException">Thrown when the file upload fails.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        /// <exception cref="NotSupportedException">Thrown when the MIME type of the URI is not supported by the API.</exception>
        public async Task<CustomLongRunningOperation> Upload(string name,
            string file,
            string? displayName,
            UploadToFileSearchStoreRequest? config = null,
            bool resumable = false,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            if (!File.Exists(file)) throw new FileNotFoundException(nameof(file));
            var fileInfo = new FileInfo(file);
            if (fileInfo.Length > Constants.MaxUploadFileSizeFileSearchStore) throw new MaxUploadFileSizeException(nameof(file));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));
            name = name.SanitizeFileSearchStoreName();
            
            var mimeType = GenerativeAIExtensions.GetMimeType(file);
            GenerativeAIExtensions.GuardMimeTypeFileSearchStore(mimeType);
            
            var request = config ?? new UploadToFileSearchStoreRequest();
            request.DisplayName ??= displayName ?? Path.GetFileNameWithoutExtension(file);
            request.MimeType ??= mimeType;

            var baseUri = BaseUrlGoogleAi.ToLowerInvariant().Replace("/{version}", "");
            var url = $"{baseUri}/upload/{Version}/{name}:uploadToFileSearchStore";   // v1beta3 // ?key={apiKey}
            if (resumable)
            { 
                url = $"{baseUri}/resumable/upload/{Version}/{name}:uploadToFileSearchStore";   // v1beta3 // ?key={apiKey}
            }
            url = ParseUrl(url).AddQueryString(new Dictionary<string, string?>()
            {
                ["alt"] = "json", 
                ["uploadType"] = "multipart"
            });
            var json = Serialize(request);

            using var fs = new FileStream(file, FileMode.Open);
            var multipartContent = new MultipartContent("related");
            multipartContent.Add(new StringContent(json, Encoding.UTF8, Constants.MediaType));
            multipartContent.Add(new StreamContent(fs, (int)Constants.ChunkSize)
            {
                Headers = { 
                    ContentType = new MediaTypeHeaderValue(mimeType), 
                    ContentLength = fileInfo.Length 
                }
            });
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = multipartContent;
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<CustomLongRunningOperation>(response);
        }

        /// <summary>
        /// Uploads data to a <see cref="FileSearchStore"/>, preprocesses and chunks before storing it in a <see cref="FileSearchStore"/> Document.
        /// </summary>
        /// <param name="name">Name of the File Search Store.</param>
        /// <param name="stream">Stream to upload.</param>
        /// <param name="displayName">A name displayed for the uploaded file.</param>
        /// <param name="mimeType">The MIME type of the stream content.</param>
        /// <param name="config">Configuration settings for the uploaded file.</param>
        /// <param name="resumable">Flag indicating whether to use resumable upload.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An operation of the uploaded file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="stream"/> is null or empty.</exception>
        /// <exception cref="MaxUploadFileSizeException">Thrown when the <paramref name="stream"/> size exceeds the maximum allowed size.</exception>
        /// <exception cref="UploadFileException">Thrown when the <paramref name="stream"/> upload fails.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        /// <exception cref="NotSupportedException">Thrown when the <paramref name="mimeType"/> is not supported by the API.</exception>
        public async Task<CustomLongRunningOperation> Upload(string name,
            Stream stream,
            string displayName,
            string mimeType,
            UploadToFileSearchStoreRequest? config = null,
            bool resumable = false,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (stream.Length > Constants.MaxUploadFileSizeFileSearchStore) throw new MaxUploadFileSizeException(nameof(stream));
            if (string.IsNullOrEmpty(mimeType)) throw new ArgumentException(nameof(mimeType));
            if (string.IsNullOrEmpty(displayName)) throw new ArgumentException(nameof(displayName));
            GenerativeAIExtensions.GuardMimeTypeFileSearchStore(mimeType);

            var totalBytes = stream.Length;
            var request = config ?? new UploadToFileSearchStoreRequest();
            request.DisplayName ??= displayName;
            request.MimeType ??= mimeType;

            var baseUri = BaseUrlGoogleAi.ToLowerInvariant().Replace("/{version}", "");
            var url = $"{baseUri}/upload/{Version}/{name}:uploadToFileSearchStore";   // v1beta3 // ?key={apiKey}
            if (resumable)
            { 
                url = $"{baseUri}/resumable/upload/{Version}/{name}:uploadToFileSearchStore";   // v1beta3 // ?key={apiKey}
            }
            url = ParseUrl(url).AddQueryString(new Dictionary<string, string?>()
            {
                ["alt"] = "json", 
                ["uploadType"] = "multipart"
            });
            var json = Serialize(request);

            var multipartContent = new MultipartContent("related");
            multipartContent.Add(new StringContent(json, Encoding.UTF8, Constants.MediaType));
            multipartContent.Add(new StreamContent(stream, (int)Constants.ChunkSize)
            {
                Headers = { 
                    ContentType = new MediaTypeHeaderValue(mimeType), 
                    ContentLength = totalBytes 
                }
            });
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = multipartContent;
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<CustomLongRunningOperation>(response);
        }

        /// <summary>
        /// Compatibility method with Google SDK. Use <see cref="Upload"/> for convenience.
        /// </summary>
        /// <param name="fileSearchStoreName">Name of the File Search Store.</param>
        /// <param name="file">URI or path to the file to upload.</param>
        /// <param name="config">Configuration settings for the uploaded file.</param>
        /// <param name="resumable">Flag indicating whether to use resumable upload.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An operation of the uploaded file.</returns>
        public async Task<CustomLongRunningOperation> UploadToFileSearchStore(
            string fileSearchStoreName,
            string file,
            UploadToFileSearchStoreRequest? config = null,
            bool resumable = false,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            return await Upload(fileSearchStoreName,
                file,
                null,
                config,
                resumable,
                requestOptions,
                cancellationToken);
        }

        /// <summary>
        /// Compatibility method with Google SDK. Use <see cref="Upload"/> for convenience.
        /// </summary>
        /// <param name="fileSearchStoreName">Name of the File Search Store.</param>
        /// <param name="stream">Stream to upload.</param>
        /// <param name="displayName">A name displayed for the uploaded file.</param>
        /// <param name="mimeType">The MIME type of the stream content.</param>
        /// <param name="config">Configuration settings for the uploaded file.</param>
        /// <param name="resumable">Flag indicating whether to use resumable upload.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An operation of the uploaded file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="stream"/> is null or empty.</exception>
        /// <exception cref="MaxUploadFileSizeException">Thrown when the <paramref name="stream"/> size exceeds the maximum allowed size.</exception>
        /// <exception cref="UploadFileException">Thrown when the <paramref name="stream"/> upload fails.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        /// <exception cref="NotSupportedException">Thrown when the <paramref name="mimeType"/> is not supported by the API.</exception>
        public async Task<CustomLongRunningOperation> UploadToFileSearchStore(string fileSearchStoreName,
            Stream stream,
            string displayName,
            string mimeType,
            UploadToFileSearchStoreRequest? config = null,
            bool resumable = false,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            return await Upload(fileSearchStoreName,
                stream,
                displayName,
                mimeType,
                config,
                resumable,
                requestOptions,
                cancellationToken);
        }
    }
}