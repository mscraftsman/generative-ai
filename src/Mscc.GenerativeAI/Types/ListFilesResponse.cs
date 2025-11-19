using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Response from ListFiles method containing a paginated list of files.
    /// </summary>
    public class ListFilesResponse
    {
        /// <summary>
        /// The list of files.
        /// </summary>
        public List<FileResource>? Files { get; set; }
        /// <summary>
        /// A token, which can be sent as pageToken to retrieve the next page.
        /// If this field is omitted, there are no more pages.
        /// </summary>
        public string? NextPageToken { get; set; }
    }
}