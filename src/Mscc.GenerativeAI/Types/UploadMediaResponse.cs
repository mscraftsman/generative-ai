#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Response from ListFiles method containing a paginated list of Files.
    /// </summary>
    public class ListFilesResponse
    {
        /// <summary>
        /// The list of Files.
        /// </summary>
        public List<FileResponse>? Files { get; set; }
        /// <summary>
        /// A token, which can be sent as pageToken to retrieve the next page.
        /// If this field is omitted, there are no more pages.
        /// </summary>
        public string? NextPageToken { get; set; }
    }
    
    /// <summary>
    /// Information about an uploaded file via FIle API
    /// Ref: https://ai.google.dev/api/rest/v1beta/files
    /// </summary>
    public class UploadMediaResponse
    {
        /// <summary>
        /// Metadata for the created file.
        /// </summary>
        public FileResponse File { get; set; }
    }
}