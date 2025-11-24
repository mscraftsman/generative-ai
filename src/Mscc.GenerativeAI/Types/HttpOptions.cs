using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// HTTP options to be used in each of the requests.
    /// </summary>
    public partial class HttpOptions
    {
        /// <summary>
        /// Specifies the version of the API to use.
        /// </summary>
        public string ApiVersion { get; set; }
        /// <summary>
        /// The base URL for the AI platform service endpoint.
        /// </summary>
        public string BaseUrl { get; set; }
        /// <summary>
        /// Additional HTTP headers to be sent with the request.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }
        /// <summary>
        /// Timeout for the request in milliseconds.
        /// </summary>
        public int Timeout { get; set; }
    }
}