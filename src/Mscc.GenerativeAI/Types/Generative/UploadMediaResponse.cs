namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Information about an uploaded file via FIle API
    /// Ref: https://ai.google.dev/api/rest/v1beta/files
    /// </summary>
    public class UploadMediaResponse
    {
        /// <summary>
        /// Metadata for the created file.
        /// </summary>
        public FileResource File { get; set; }

        public string Name => File.Name;
        public string Uri => File.Uri;
        public string MimeType => File.MimeType;
    }
}