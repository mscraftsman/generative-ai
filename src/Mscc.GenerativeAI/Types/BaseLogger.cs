using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Abstract base type with logging instance.
    /// </summary>
    public abstract class BaseLogger
    {
        protected ILogger Logger { get; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected BaseLogger() : this(null) { }

        /// <summary>
        /// Base constructor to set the <see cref="ILogger"/> instance.
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        protected BaseLogger(ILogger? logger) => Logger = logger ?? NullLogger.Instance;
    }
}