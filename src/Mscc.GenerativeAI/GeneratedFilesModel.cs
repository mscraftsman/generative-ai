#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
#endif
using Microsoft.Extensions.Logging;

namespace Mscc.GenerativeAI
{
    public sealed class GeneratedFilesModel : BaseModel
    {
        protected override string Version => ApiVersion.V1Beta;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratedFilesModel"/> class.
        /// </summary>
        public GeneratedFilesModel() : this(logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratedFilesModel"/> class.
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public GeneratedFilesModel(ILogger? logger) : base(logger) { }
        
        /// <summary>
        /// Lists the generated files owned by the requesting project.
        /// </summary>
        /// <param name="pageSize">The maximum number of Models to return (per page).</param>
        /// <param name="pageToken">A page token, received from a previous ListFiles call. Provide the pageToken returned by one request as an argument to the next request to retrieve the next page.</param>
        /// <returns>List of files in File API.</returns>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<ListGeneratedFilesResponse> ListFiles(int? pageSize = 100, 
            string? pageToken = null)
        {
            // this.GuardSupported();
            
            var url = $"{BaseUrlGoogleAi}/generatedFiles";
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(pageSize)] = Convert.ToString(pageSize), 
                [nameof(pageToken)] = pageToken
            };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            var response = await Client.GetAsync(url);
            await response.EnsureSuccessAsync();
            return await Deserialize<ListGeneratedFilesResponse>(response);
        }
        
    }
}