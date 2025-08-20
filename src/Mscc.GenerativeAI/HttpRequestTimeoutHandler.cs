#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
#endif
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
        
        public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(100);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="TimeoutException"></exception>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            using (var cts = GetCancellationTokenSource(request, cancellationToken))
            {
                try
                {
                    return await base.SendAsync(request, cts?.Token ?? cancellationToken);
                }
                catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    throw new TimeoutException();
                }
                catch (InvalidOperationException) when (!cancellationToken.IsCancellationRequested)
                {
                    throw new TimeoutException();
                }
                catch (TaskCanceledException ex) when (cancellationToken.IsCancellationRequested)
                {
                    // Handle case where the operation is canceled explicitly
                    Logger.LogWarning(ex.Message);
                    throw;
                }
                catch (HttpRequestException ex)
                {
                    // Handle network or request-level exceptions (e.g., 5xx errors)
                    // Optionally log ex
                    Logger.LogWarning(ex.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    // Catch any unexpected exceptions, could be logged
                    Logger.LogError(ex.Message);
                    throw;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private CancellationTokenSource? GetCancellationTokenSource(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var timeout = request.GetTimeout() ?? DefaultTimeout;
            if (timeout == Timeout.InfiniteTimeSpan)
            {
                return null;
            }
            else
            {
                var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(timeout);
                return cts;
            }
        }
    }
}