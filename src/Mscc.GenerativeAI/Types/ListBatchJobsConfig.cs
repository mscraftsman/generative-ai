namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Config for optional parameters.
    /// </summary>
    public partial class ListBatchJobsConfig : BaseConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Filter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? PageSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? PageToken { get; set; }
    }
}