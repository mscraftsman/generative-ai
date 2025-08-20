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
        public Retry Retry { get; set; }
        
        /// <summary>
        /// Gets or sets the timeout for this specific request. 
        /// If set to a positive value, the request will be cancelled if it exceeds this duration.
        /// This is achieved by linking a CancellationToken to the request.
        /// A value of TimeSpan.Zero (the default) or a negative value means no per-request timeout will be applied beyond any default client timeout.
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestOptions"/> class
        /// </summary>
        public RequestOptions()
        {
            Retry = new Retry();
            Timeout = TimeSpan.FromSeconds(100);    // default value of HttpClient
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestOptions"/> class.
        /// </summary>
        /// <param name="timeout">Optional. The timeout duration for the request in seconds. 
        /// If null, or if TimeSpan.Zero or a negative value is provided, no specific per-request timeout is applied.
        /// </param>
        public RequestOptions(TimeSpan? timeout = null) : this()
        {
            Retry = new Retry();
            Timeout = timeout ?? Timeout;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestOptions"/> class
        /// </summary>
        /// <param name="retry">Refer to [retry docs](https://googleapis.dev/python/google-api-core/latest/retry.html) for details.</param>
        /// <param name="timeout">In seconds (or provide a [TimeToDeadlineTimeout](https://googleapis.dev/python/google-api-core/latest/timeout.html) object).</param>
        public RequestOptions(Retry? retry, TimeSpan? timeout = null) : this()
        {
            Retry = retry ?? Retry;
            Timeout = timeout ?? Timeout;
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