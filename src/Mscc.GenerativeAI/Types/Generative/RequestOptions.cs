using System;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Provides options for individual API requests.
    /// </summary>
    public class RequestOptions
    {
        private HttpRequestHeaders _headers;

        /// <summary>
        /// Gets or sets the <see cref="Retry"/> options for this request.
        /// </summary>
        public Retry Retry { get; set; }

        /// <summary>
        /// Gets or sets the timeout for this specific request. 
        /// If set to a positive value, the request will be cancelled if it exceeds this duration.
        /// This is achieved by linking a CancellationToken to the request.
        /// A value of TimeSpan.Zero (the default) or a negative value means no per-request timeout will be applied beyond any default client timeout.
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// The base URL to use for the request.
        /// If not set, the default base URL for the model will be used.
        /// </summary>
        public string? BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the version of the API.
        /// </summary>
        public string? ApiVersion { get; set; }

        /// <summary>
        /// Gets or sets the proxy to use for the request.
        /// </summary>
        public IWebProxy? Proxy { get; set; }

        /// <summary>
        /// Gets or sets the headers to use for the request.
        /// </summary>
        public HttpRequestHeaders Headers => _headers ?? (_headers = new HttpRequestMessage().Headers);

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestOptions"/> class
        /// </summary>
        public RequestOptions()
        {
            Retry = new Retry();
            Timeout = TimeSpan.FromSeconds(100); // default value of HttpClient
            // ApiVersion = Mscc.GenerativeAI.ApiVersion.V1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestOptions"/> class
        /// </summary>
        /// <param name="retry">Optional. Refer to [retry docs](https://googleapis.dev/python/google-api-core/latest/retry.html) for details.</param>
        /// <param name="timeout">Optional. In seconds (or provide a [TimeToDeadlineTimeout](https://googleapis.dev/python/google-api-core/latest/timeout.html) object).</param>
        /// <param name="baseUrl">Optional. The base URL to use for the request.</param>
        /// <param name="apiVersion">Optional. The version of the API.</param>
        /// <param name="proxy">Optional. Proxy settings to use for the request.</param>
        public RequestOptions(Retry? retry = null,
            TimeSpan? timeout = null,
            string? baseUrl = null,
            string? apiVersion = null,
            IWebProxy? proxy = null) : this()
        {
            Retry = retry ?? Retry;
            Timeout = timeout ?? Timeout;
            BaseUrl = baseUrl;
            ApiVersion = apiVersion;
            Proxy = proxy;
        }
    }

    /// <summary>
    /// Defines the retry strategy for a request.
    /// </summary>
    public sealed class Retry
    {
        /// <summary>
        /// The initial delay before the first retry, in seconds.
        /// </summary>
        public int Initial { get; set; } = 1;

        /// <summary>
        /// The multiplier for the delay between retries.
        /// </summary>
        public int Multiplies { get; set; } = 2;

        /// <summary>
        /// The maximum number of retries.
        /// </summary>
        public int Maximum { get; set; } = 60;

        /// <summary>
        /// The overall timeout for the retry logic.
        /// </summary>
        public TimeSpan? Timeout { get; set; } = TimeSpan.FromSeconds(301);

        /// <summary>
        /// The HTTP status codes that should trigger a retry.
        /// </summary>
        public int[]? StatusCodes { get; set; } = Constants.RetryStatusCodes;
    }
}