namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CountTokensResponse
    {
        private int _totalTokens = default;

        /// <summary>
        /// The total number of tokens counted across all instances from the request.
        /// </summary>
        public int TotalTokens
        {
            get => _totalTokens;
            set => _totalTokens = value < 0 ? 0 : value < int.MaxValue ? value : int.MaxValue;
        }

        /// <summary>
        /// The total number of tokens counted across all instances from the request.
        /// </summary>
        public int TokenCount
        {
            get => _totalTokens;
            set => _totalTokens = value < 0 ? 0 : value < int.MaxValue ? value : int.MaxValue;
        }

        /// <summary>
        /// The total number of billable characters counted across all instances from the request.
        /// </summary>
        public int TotalBillableCharacters { get; set; } = default;
    }
}