namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Request to copy a model.
    /// </summary>
    public class CopyModelRequest
    {
        /// <summary>
        /// The Google Cloud path of the source model.
        /// </summary>
        /// <remarks>
        /// The path is based on "projects/SOURCE_PROJECT_ID/locations/SOURCE_LOCATION/models/SOURCE_MODEL_ID[@VERSION_ID]"
        /// </remarks>
        public string SourceModel { get; set; }
    }
}