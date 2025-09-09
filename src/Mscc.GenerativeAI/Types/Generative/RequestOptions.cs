#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Provides options for individual API requests.
    /// </summary>
    public class RequestOptions
    {
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
        /// Initializes a new instance of the <see cref="RequestOptions"/> class
        /// </summary>
        public RequestOptions()
        {
            Retry = new Retry();
            Timeout = TimeSpan.FromSeconds(100);    // default value of HttpClient
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestOptions"/> class
        /// </summary>
        /// <param name="retry">Optional. Refer to [retry docs](https://googleapis.dev/python/google-api-core/latest/retry.html) for details.</param>
        /// <param name="timeout">Optional. In seconds (or provide a [TimeToDeadlineTimeout](https://googleapis.dev/python/google-api-core/latest/timeout.html) object).</param>
        /// <param name="baseUrl">Optional. The base URL to use for the request.</param>
        public RequestOptions(Retry? retry = null, TimeSpan? timeout = null, string? baseUrl = null) : this()
        {
            Retry = retry ?? Retry;
            Timeout = timeout ?? Timeout;
            BaseUrl = baseUrl;
        }
    }

    /// <summary>
    /// Defines the retry strategy for a request.
    /// </summary>
    public class Retry
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