#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A resource representing a batch of GenerateContent requests.
    /// </summary>
    public class GenerateContentBatch
    {
        /// <summary>
        /// Required. The name of the `Model` to use for generating the completion. Format: `models/{model}`.
        /// </summary>
        public string? Model { get; set; }
        /// <summary>
        /// Required. Input configuration of the instances on which batch processing are performed.
        /// </summary>
        public InputConfig InputConfig { get; set; }
        /// <summary>
        /// Required. The user-defined name of this batch.
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Output only. Identifier. Resource name of the batch. Format: `batches/{batch_id}`.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Optional. The priority of the batch.
        /// </summary>
        /// <remarks>
        /// Batches with a higher priority value will be processed before batches with a lower priority value.
        /// Negative values are allowed. Default is 0.
        /// </remarks>
        public int Priority { get; set; }
        /// <summary>
        /// Output only. The state of the batch.
        /// </summary>
        public BatchState State { get; set; }
        /// <summary>
        /// Output only. Stats about the batch.
        /// </summary>
        public BatchStats BatchStats { get; set; }
        /// <summary>
        /// Output only. The time at which the batch was created.
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Output only. The time at which the batch was last updated.
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// Output only. The time at which the batch processing completed.
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// Output only. The output of the batch request.
        /// </summary>
        public GenerateContentBatchOutput Output { get; set; }
    }
}