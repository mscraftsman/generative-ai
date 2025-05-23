#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
#endif
using Microsoft.Extensions.Logging;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Entry point to access Gemini API running in Google AI.
    /// </summary>
    /// <remarks>
    /// See <a href="https://ai.google.dev/api/rest">Model reference</a>.
    /// </remarks>
    public sealed class GoogleAI : BaseLogger, IGenerativeAI
    {
        private readonly string? _apiKey;
        private readonly string? _accessToken;
        private readonly string _version;
        private GenerativeModel? _generativeModel;
        private FilesModel? _filesModel;
        private MediaModel? _mediaModel;
        private GeneratedFilesModel? _generatedFilesModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleAI"/> class with access to Google AI Gemini API.
        /// The default constructor attempts to read <c>.env</c> file and environment variables.
        /// Sets default values, if available.
        /// </summary>
        /// <remarks>The following environment variables are used:
        /// <list type="table">
        /// <item><term>GOOGLE_API_KEY</term>
        /// <description>API key provided by Google AI Studio.</description></item>
        /// <item><term>GOOGLE_ACCESS_TOKEN</term>
        /// <description>Optional. Access token provided by OAuth 2.0 or Application Default Credentials (ADC).</description></item>
        /// </list>
        /// </remarks>
        private GoogleAI(ILogger? logger = null) : base(logger)
        {
            GenerativeAIExtensions.ReadDotEnv();
            _apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY") ??
                      Environment.GetEnvironmentVariable("GEMINI_API_KEY");
            _accessToken = Environment.GetEnvironmentVariable("GOOGLE_ACCESS_TOKEN");
            _version = ApiVersion.V1Beta;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleAI"/> class with access to Google AI Gemini API.
        /// Either API key or access token is required.
        /// </summary>
        /// <param name="apiKey">API key for Google AI Studio.</param>
        /// <param name="accessToken">Access token for the Google Cloud project.</param>
        /// <param name="apiVersion">Version of the API.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public GoogleAI(string? apiKey = null, string? accessToken = null, string? apiVersion = null, ILogger? logger = null) : this(logger)
        {
            _apiKey = apiKey ?? _apiKey;
            _accessToken = accessToken ?? _accessToken;
            _version = apiVersion ?? _version;
        }

        /// <summary>
        /// Create a generative model on Google AI to use.
        /// </summary>
        /// <param name="model">Model to use (default: "gemini-1.5-pro")</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <param name="systemInstruction">Optional. </param>
        /// <returns>Generative model instance.</returns>
        public GenerativeModel GenerativeModel(string model = Model.Gemini15Pro,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null,
            Content? systemInstruction = null,
            ILogger? logger = null)
        {
            Guard();
            
            _generativeModel = new GenerativeModel(_apiKey,
                model,
                generationConfig,
                safetySettings,
                tools,
                systemInstruction,
                logger: Logger)
            {
                AccessToken = _apiKey is null ? _accessToken : null, 
                Version = _version
            };
            return _generativeModel;
        }

        /// <summary>
        /// Create a generative model on Google AI to use.
        /// </summary>
        /// <param name="cachedContent">Content that has been preprocessed.</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <returns>Generative model instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="cachedContent"/> is null.</exception>
        public GenerativeModel GenerativeModel(CachedContent cachedContent,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            ILogger? logger = null)
        {
            if (cachedContent == null) throw new ArgumentNullException(nameof(cachedContent));
            Guard();

            _generativeModel = new GenerativeModel(cachedContent,
                generationConfig,
                safetySettings,
                logger: Logger)
            {
                ApiKey = _apiKey,
                AccessToken = _apiKey is null ? _accessToken : null
            };
            return _generativeModel;
        }

        /// <inheritdoc cref="IGenerativeAI"/>
        public async Task<ModelResponse> GetModel(string model, CancellationToken cancellationToken = default)
        {
            return await _generativeModel?.GetModel(model, cancellationToken)!;
        }

        /// <summary>
        /// Returns an instance of CachedContent to use with a model.
        /// </summary>
        /// <returns>Cached content instance.</returns>
        public CachedContentModel CachedContent(
        ILogger? logger = null)
        {
            Guard();

            var cachedContent = new CachedContentModel(logger: logger) 
            {
                ApiKey = _apiKey,
                AccessToken = _apiKey is null ? _accessToken : null
            };
            return cachedContent;
        }

        /// <summary>
        /// Returns an instance of <see cref="ImageGenerationModel"/> to use with a model.
        /// </summary>
        /// <param name="model">Model to use (default: "imagegeneration")</param>
        /// <returns>Imagen model</returns>
        public ImageGenerationModel ImageGenerationModel(string model = Model.Imagen3,
            ILogger? logger = null)
        {
            Guard();

            var imageGenerationModel = new ImageGenerationModel(apiKey: _apiKey, model: model, logger: logger)
            {
                AccessToken = _apiKey is null ? _accessToken : null
            };
            return imageGenerationModel;
        }

        /// <summary>
        /// Uploads a file to the File API backend.
        /// </summary>
        /// <param name="uri">URI or path to the file to upload.</param>
        /// <param name="displayName">A name displayed for the uploaded file.</param>
        /// <param name="resumable">Flag indicating whether to use resumable upload.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the upload.</param>
        /// <returns>A URI of the uploaded file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="uri"/> is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the file <paramref name="uri"/> is not found.</exception>
        /// <exception cref="MaxUploadFileSizeException">Thrown when the file size exceeds the maximum allowed size.</exception>
        /// <exception cref="UploadFileException">Thrown when the file upload fails.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<UploadMediaResponse> UploadFile(string uri,
            string? displayName = null,
            bool resumable = false,
            CancellationToken cancellationToken = default)
        {
            Guard();

            _mediaModel ??= new()
            {
                ApiKey = _apiKey, 
                AccessToken = _apiKey is null ? _accessToken : null
            };
            return await _mediaModel?.UploadFile(uri, displayName, resumable, cancellationToken)!;
        }

        /// <summary>
        /// Uploads a stream to the File API backend.
        /// </summary>
        /// <param name="stream">Stream to upload.</param>
        /// <param name="displayName">A name displayed for the uploaded file.</param>
        /// <param name="mimeType">The MIME type of the stream content.</param>
        /// <param name="resumable">Flag indicating whether to use resumable upload.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the upload.</param>
        /// <returns>A URI of the uploaded file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="stream"/> is null or empty.</exception>
        /// <exception cref="MaxUploadFileSizeException">Thrown when the <paramref name="stream"/> size exceeds the maximum allowed size.</exception>
        /// <exception cref="UploadFileException">Thrown when the <paramref name="stream"/> upload fails.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<UploadMediaResponse> UploadFile(Stream stream,
            string displayName,
            string mimeType,
            bool resumable = false,
            CancellationToken cancellationToken = default)
        {
            Guard();

            _mediaModel ??= new()
            {
                ApiKey = _apiKey, 
                AccessToken = _apiKey is null ? _accessToken : null
            };
            return await _mediaModel?.UploadFile(stream, displayName, mimeType, resumable, cancellationToken)!;
        }

        /// <summary>
        /// Gets a generated file.
        /// </summary>
        /// <remarks>
        /// When calling this method via REST, only the metadata of the generated file is returned.
        /// To retrieve the file content via REST, add alt=media as a query parameter.
        /// </remarks>
        /// <param name="file">Required. The name of the generated file to retrieve. Example: `generatedFiles/abc-123`</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Metadata for the given file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="file"/> is null or empty.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<GeneratedFile> DownloadFile(string file, CancellationToken cancellationToken = default)
        {
            Guard();

            _mediaModel ??= new()
            {
                ApiKey = _apiKey, 
                AccessToken = _apiKey is null ? _accessToken : null
            };
            return await _mediaModel.DownloadFile(file, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Lists the metadata for Files owned by the requesting project.
        /// </summary>
        /// <param name="pageSize">The maximum number of Models to return (per page).</param>
        /// <param name="pageToken">A page token, received from a previous files.list call. Provide the pageToken returned by one request as an argument to the next request to retrieve the next page.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>List of files in File API.</returns>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<ListFilesResponse> ListFiles(int? pageSize = 100,
            string? pageToken = null, CancellationToken cancellationToken = default)
        {
            Guard();

            _filesModel ??= new()
            {
                ApiKey = _apiKey, 
                AccessToken = _apiKey is null ? _accessToken : null
            };
            return await _filesModel?.ListFiles(pageSize, pageToken, cancellationToken)!;
        }

        /// <summary>
        /// Gets the metadata for the given File.
        /// </summary>
        /// <param name="file">Required. The resource name of the file to get. This name should match a file name returned by the files.list method. Format: files/file-id.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Metadata for the given file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="file"/> is null or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<FileResource> GetFile(string file, CancellationToken cancellationToken = default)
        {
            Guard();

            _filesModel ??= new()
            {
                ApiKey = _apiKey, 
                AccessToken = _apiKey is null ? _accessToken : null
            };
            return await _filesModel?.GetFile(file, cancellationToken)!;
        }

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="file">Required. The resource name of the file to get. This name should match a file name returned by the files.list method. Format: files/file-id.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>If successful, the response body is empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="file"/> is null or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<string> DeleteFile(string file, CancellationToken cancellationToken = default)
        {
            Guard();

            _filesModel ??= new()
            {
                ApiKey = _apiKey, 
                AccessToken = _apiKey is null ? _accessToken : null
            };
            return await _filesModel?.DeleteFile(file, cancellationToken)!;
        }

        /// <summary>
        /// Lists the metadata for Files owned by the requesting project.
        /// </summary>
        /// <param name="pageSize">The maximum number of Models to return (per page).</param>
        /// <param name="pageToken">A page token, received from a previous files.list call. Provide the pageToken returned by one request as an argument to the next request to retrieve the next page.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>List of files in File API.</returns>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<ListGeneratedFilesResponse> ListGeneratedFiles(int? pageSize = 100,
            string? pageToken = null, CancellationToken cancellationToken = default)
        {
            Guard();

            _generatedFilesModel ??= new()
            {
                ApiKey = _apiKey, 
                AccessToken = _apiKey is null ? _accessToken : null
            };
            return await _generatedFilesModel?.ListFiles(pageSize, pageToken, cancellationToken)!;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when both "apiKey" and "accessToken" are <see langword="null"/>.</exception>
        private void Guard()
        {
            if (_apiKey is null && _accessToken is null) 
                throw new ArgumentNullException(message: "Either API key or access token is required.", null);
        }
    }
}
