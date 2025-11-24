namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Config for optional parameters.
    /// </summary>
    public partial class CreateBatchJobConfig : BaseConfig
    {
        /// <summary>
        /// GCS or BigQuery URI prefix for the output predictions. Example: “gs://path/to/output/data” or “bq://projectId.bqDatasetId.bqTableId”.
        /// </summary>
        public string? Dest { get; set; }
        /// <summary>
        /// The user-defined name of this BatchJob.
        /// </summary>
        public string? DisplayName { get; set; }
    }
}