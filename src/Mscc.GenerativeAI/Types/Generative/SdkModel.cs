namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The model object.
    /// </summary>
    public class SdkModel
    {
        /// <summary>
        /// Output only. Id of the model.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Output only. Always "model", required by the SDK.
        /// </summary>
        public string Object { get; set; }
        /// <summary>
        /// Output only. The Unix timestamp (in seconds) when the model was created.
        /// </summary>
        public long Created { get; set; }
        /// <summary>
        /// "Output only. The organization that owns the model.
        /// </summary>
        public string OwnedBy { get; set; }
    }
}