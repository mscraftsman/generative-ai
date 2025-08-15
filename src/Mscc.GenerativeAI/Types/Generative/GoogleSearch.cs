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
    }
}