using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Mscc.GenerativeAI
{
    public abstract class GenerationBase
    {
        protected ILogger Logger { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger">Logger instance used for logging (optional)</param>
        protected GenerationBase(ILogger? logger)
        {
            this.Logger = logger ?? NullLogger.Instance;
        }
    }
}