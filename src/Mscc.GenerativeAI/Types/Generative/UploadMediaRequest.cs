namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Instance to upload a local file to create a File resource.
    /// </summary>
    public class UploadMediaRequest
    {
        /// <summary>
        /// Optional. Metadata for the file to create.
        /// </summary>
        public FileRequest File { get; set; }
    }
}