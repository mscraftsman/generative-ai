#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

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
    }
}