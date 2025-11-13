using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A delegating handler that provides a timeout for HTTP requests.
    /// </summary>
    public class HttpRequestTimeoutHandler : DelegatingHandler
    {
        protected ILogger Logger { get; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        internal HttpRequestTimeoutHandler() : this(null) { }

        /// <summary>
        /// Base constructor to set the <see cref="ILogger"/> instance.
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        internal HttpRequestTimeoutHandler(ILogger? logger) => Logger = logger ?? NullLogger.Instance;
        
        private CancellationTokenSource? GetCancellationTokenSource(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var timeout = request.GetTimeout();
            if (timeout.HasValue && timeout.Value != Timeout.InfiniteTimeSpan)
            {
                var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(timeout.Value);
                return cts;
            }
            return null;
        }
    }
}