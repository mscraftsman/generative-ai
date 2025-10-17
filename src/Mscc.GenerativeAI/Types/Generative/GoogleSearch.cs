#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// GoogleSearch tool type. Tool to support Google Search in Model. Powered by Google.
    /// </summary>
    public class GoogleSearch
    {
        /// <summary>
        /// Optional. Filter search results to a specific time range.
        /// If customers set a start time, they must set an end time (and vice versa).
        /// </summary>
        public Interval? TimeRangeFilter { get; set; }
        /// <summary>
        /// Optional. Sites with confidence level chosen and above this value will be blocked from the search results.
        /// </summary>
        public BlockingConfidence? BlockingConfidence { get; set; }
        /// <summary>
        /// Optional. List of domains to be excluded from the search results. The default limit is 2000 domains. Example: ["amazon.com", "facebook.com"].
        /// </summary>
        public List<string>? ExcludeDomains { get; set; }
    }
}