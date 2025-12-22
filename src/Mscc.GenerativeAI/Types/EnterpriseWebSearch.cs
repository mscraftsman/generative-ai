namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Tool to search public web data, powered by Vertex AI Search and Sec4 compliance.
    /// </summary>
    public partial class EnterpriseWebSearch : ITool
    {
        /// <summary>
        /// Optional. Filter search results to a specific time range. If customers set a start time,
        /// they must set an end time (and vice versa). This field is not supported in Vertex AI.
        /// </summary>  
        public Interval? TimeRangeFilter { get; set; }
    }
}