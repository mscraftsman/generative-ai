namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Abstract base class for many config types.
    /// </summary>
    public abstract class BaseConfig
    {
        /// <summary>
        /// Used to override HTTP request options.
        /// </summary>
        public HttpOptions? HttpOptions { get; set; }
        /// <summary>
        /// Whether to include a reason for filtered-out images in the response.
        /// </summary>
        public bool? IncludeRaiReason { get; set; }
    }
}