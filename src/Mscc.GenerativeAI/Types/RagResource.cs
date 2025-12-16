namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// The definition of the Rag resource.
    /// </summary>
    public partial class RagResource
    {
        /// <summary>
        /// Optional. RagCorpora resource name.
        /// </summary>
        /// <remarks>
        /// Format: projects/{project}/locations/{location}/ragCorpora/{ragCorpus}
        /// </remarks>
        public string? RagCorpus { get; set; }
        /// <summary>
        /// Optional. ragFileId. The files should be in the same ragCorpus set in ragCorpus field.
        /// </summary>
        public string[]? RagFileIds { get; set; }
    }
}