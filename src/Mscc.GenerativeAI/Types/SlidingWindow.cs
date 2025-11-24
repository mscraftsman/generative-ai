namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// The SlidingWindow method operates by discarding content at the beginning of the context window. The resulting context will always begin at the start of a USER role turn. System instructions and any `BidiGenerateContentSetup.prefix_turns` will always remain at the beginning of the result.
    /// </summary>
    public partial class SlidingWindow
    {
        /// <summary>
        /// The target number of tokens to keep. The default value is trigger_tokens/2. Discarding parts of the context window causes a temporary latency increase so this value should be calibrated to avoid frequent compression operations.
        /// </summary>
        public int TargetTokens { get; set; }
    }
}