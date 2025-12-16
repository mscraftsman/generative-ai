namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Configuration options that change client network behavior when testing.
    /// </summary>
    public partial class DebugConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public string ClientMode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReplayId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReplaysDirectory { get; set; }
    }
}