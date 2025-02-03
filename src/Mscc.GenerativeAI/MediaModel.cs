#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
#endif
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;

namespace Mscc.GenerativeAI
{
    public sealed class MediaModel : BaseModel
    {
        internal override string Version => ApiVersion.V1Beta;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaModel"/> class.
        /// </summary>
        public MediaModel() : this(logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaModel"/> class.
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public MediaModel(ILogger? logger) : base(logger) { }

        /// <summary>
        /// Uploads a file to the File API backend.
        /// </summary>
        /// <param name="uri">URI or path to the file to upload.</param>
        /// <param name="displayName">A name displayed for the uploaded file.</param>
        /// <param name="resumable">Flag indicating whether to use resumable upload.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A URI of the uploaded file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="uri"/> is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the file <paramref name="uri"/> is not found.</exception>
        /// <exception cref="MaxUploadFileSizeException">Thrown when the file size exceeds the maximum allowed size.</exception>
        /// <exception cref="UploadFileException">Thrown when the file upload fails.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        /// <exception cref="NotSupportedException">Thrown when the MIME type of the URI is not supported by the API.</exception>
        public async Task<UploadMediaResponse> UploadFile(string uri,
            string? displayName = null,
            bool resumable = false,
            CancellationToken cancellationToken = default)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));
            if (!File.Exists(uri)) throw new FileNotFoundException(nameof(uri));
            var fileInfo = new FileInfo(uri);
            if (fileInfo.Length > Constants.MaxUploadFileSize) throw new MaxUploadFileSizeException(nameof(uri));

            var mimeType = GenerativeAIExtensions.GetMimeType(uri);
            GenerativeAIExtensions.GuardMimeType(mimeType);
            
            var totalBytes = new FileInfo(uri).Length;
            var request = new UploadMediaRequest()
            {
                File = new FileRequest()
                {
                    DisplayName = displayName ?? Path.GetFileNameWithoutExtension(uri),
                }
            };

            var baseUri = BaseUrlGoogleAi.ToLowerInvariant().Replace("/{version}", "");
            var url = $"{baseUri}/upload/{Version}/files";   // v1beta3 // ?key={apiKey}
            if (resumable)
            { 
                url = $"{baseUri}/resumable/upload/{Version}/files";   // v1beta3 // ?key={apiKey}
            }
            url = ParseUrl(url).AddQueryString(new Dictionary<string, string?>()
            {
                ["alt"] = "json", 
                ["uploadType"] = "multipart"
            });
            var json = Serialize(request);

            using var fs = new FileStream(uri, FileMode.Open);
            var multipartContent = new MultipartContent("related");
            multipartContent.Add(new StringContent(json, Encoding.UTF8, Constants.MediaType));
            multipartContent.Add(new StreamContent(fs, (int)Constants.ChunkSize)
            {
                Headers = { 
                    ContentType = new MediaTypeHeaderValue(mimeType), 
                    ContentLength = totalBytes 
                }
            });
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = multipartContent;
            var response = await SendAsync(httpRequest, cancellationToken);
            await response.EnsureSuccessAsync();
            return await Deserialize<UploadMediaResponse>(response);
        }

        /// <summary>
        /// Uploads a stream to the File API backend.
        /// </summary>
        /// <param name="stream">Stream to upload.</param>
        /// <param name="displayName">A name displayed for the uploaded file.</param>
        /// <param name="mimeType">The MIME type of the stream content.</param>
        /// <param name="resumable">Flag indicating whether to use resumable upload.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A URI of the uploaded file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="stream"/> is null or empty.</exception>
        /// <exception cref="MaxUploadFileSizeException">Thrown when the <paramref name="stream"/> size exceeds the maximum allowed size.</exception>
        /// <exception cref="UploadFileException">Thrown when the <paramref name="stream"/> upload fails.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        /// <exception cref="NotSupportedException">Thrown when the <paramref name="mimeType"/> is not supported by the API.</exception>
        public async Task<UploadMediaResponse> UploadFile(Stream stream,
            string displayName,
            string mimeType,
            bool resumable = false,
            CancellationToken cancellationToken = default)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (stream.Length > Constants.MaxUploadFileSize) throw new MaxUploadFileSizeException(nameof(stream));
            if (string.IsNullOrEmpty(mimeType)) throw new ArgumentException(nameof(mimeType));
            if (string.IsNullOrEmpty(displayName)) throw new ArgumentException(nameof(displayName));
            GenerativeAIExtensions.GuardMimeType(mimeType);

            var totalBytes = stream.Length;
            var request = new UploadMediaRequest()
            {
                File = new FileRequest()
                {
                    DisplayName = displayName
                }
            };

            var baseUri = BaseUrlGoogleAi.ToLowerInvariant().Replace("/{version}", "");
            var url = $"{baseUri}/upload/{Version}/files";   // v1beta3 // ?key={apiKey}
            if (resumable)
            { 
                url = $"{baseUri}/resumable/upload/{Version}/files";   // v1beta3 // ?key={apiKey}
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
            var response = await SendAsync(httpRequest, cancellationToken);
            await response.EnsureSuccessAsync();
            return await Deserialize<UploadMediaResponse>(response);
        }

        /// <summary>
        /// Gets a generated file.
        /// </summary>
        /// <remarks>
        /// When calling this method via REST, only the metadata of the generated file is returned.
        /// To retrieve the file content via REST, add alt=media as a query parameter.
        /// </remarks>
        /// <param name="file">Required. The name of the generated file to retrieve. Example: `generatedFiles/abc-123`</param>
        /// <param name="media">Optional. Flag indicating whether to retrieve the file content.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Metadata for the given file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="file"/> is null or empty.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<GeneratedFile> DownloadFile(string file,
            bool media = false,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(file)) throw new ArgumentNullException(nameof(file));

            file = file.SanitizeGeneratedFileName();

            var url = $"{BaseUrlGoogleAi}/{file}";
            url = ParseUrl(url);
            if (media)
            {
                url.AddQueryString(new Dictionary<string, string?>()
                {
                    ["alt"] = "media"
                });
            }
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, cancellationToken);
            await response.EnsureSuccessAsync();
            return await Deserialize<GeneratedFile>(response);
        }
    }
}