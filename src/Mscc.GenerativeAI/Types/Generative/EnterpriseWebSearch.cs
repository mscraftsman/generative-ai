using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Tool to search public web data, powered by Vertex AI Search and Sec4 compliance.
    /// </summary>
    public class EnterpriseWebSearch
    {
        /// <summary>
        /// Optional. List of domains to be excluded from the search results.
        /// The default limit is 2000 domains.
        /// </summary>
        public List<string>? ExcludeDomains { get; set; }
        /// <summary>
        /// Optional. Sites with confidence level chosen and above this value will be blocked from the search results.
        /// </summary>
        public PhishBlockThreshold? BlockingConfidence { get; set; }
        /// <summary>
        /// Optional. Filter search results to a specific time range. If customers set a start time,
        /// they must set an end time (and vice versa). This field is not supported in Vertex AI.
        /// </summary>  
        public Interval? TimeRangeFilter { get; set; }
    }
}