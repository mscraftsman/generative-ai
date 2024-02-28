namespace Mscc.GenerativeAI
{
    public class CountTokensResponse
    {
        /// <summary>
        /// The total number of tokens counted across all instances from the request.
        /// </summary>
        public int TotalTokens { get; set; } = default;

        /// <summary>
        /// The total number of billable characters counted across all instances from the request.
        /// </summary>
        public int TotalBillableCharacters { get; set; } = default;
    }
}