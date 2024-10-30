#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
#endif
using Microsoft.Extensions.Logging;

namespace Mscc.GenerativeAI
{
    public sealed class FilesModel : BaseModel
    {
        protected override string Version => ApiVersion.V1Beta;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesModel"/> class.
        /// </summary>
        public FilesModel() : this(logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesModel"/> class.
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public FilesModel(ILogger? logger) : base(logger) { }
        
        /// <summary>
        /// Lists the metadata for Files owned by the requesting project.
        /// </summary>
        /// <param name="pageSize">The maximum number of Models to return (per page).</param>
        /// <param name="pageToken">A page token, received from a previous ListFiles call. Provide the pageToken returned by one request as an argument to the next request to retrieve the next page.</param>
        /// <returns>List of files in File API.</returns>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<ListFilesResponse> ListFiles(int? pageSize = 100, 
            string? pageToken = null)
        {
            // this.GuardSupported();
            
            var url = "{endpointGoogleAI}/{Version}/files";
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(pageSize)] = Convert.ToString(pageSize), 
                [nameof(pageToken)] = pageToken
            };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            var response = await Client.GetAsync(url);
            await response.EnsureSuccessAsync();
            return await Deserialize<ListFilesResponse>(response);
        }

        /// <summary>
        /// Gets the metadata for the given File.
        /// </summary>
        /// <param name="file">Required. The resource name of the file to get. This name should match a file name returned by the ListFiles method. Format: files/file-id.</param>
        /// <returns>Metadata for the given file.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="file"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<FileResource> GetFile(string file)
        {
            if (string.IsNullOrEmpty(file)) throw new ArgumentException("Value cannot be null or empty.", nameof(file));
            // this.GuardSupported();

            file = file.SanitizeFileName();

            var url = $"{EndpointGoogleAi}/{Version}/{file}";
            url = ParseUrl(url);
            var response = await Client.GetAsync(url);
            await response.EnsureSuccessAsync();
            return await Deserialize<FileResource>(response);
        }

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="file">Required. The resource name of the file to get. This name should match a file name returned by the ListFiles method. Format: files/file-id.</param>
        /// <returns>If successful, the response body is empty.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="file"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<string> DeleteFile(string file)
        {
            if (string.IsNullOrEmpty(file)) throw new ArgumentException("Value cannot be null or empty.", nameof(file));
            // this.GuardSupported();

            file = file.SanitizeFileName();

            var url = $"{EndpointGoogleAi}/{Version}/{file}";   // v1beta3
            url = ParseUrl(url);
            var response = await Client.DeleteAsync(url);
            await response.EnsureSuccessAsync();
            return await response.Content.ReadAsStringAsync();
        }
    }
}