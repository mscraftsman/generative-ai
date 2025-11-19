using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Response from ListFiles method containing a paginated list of generated files.
    /// </summary>
    public class ListGeneratedFilesResponse
    {
        /// <summary>
        /// The list of generated files.
        /// </summary>
        public List<FileResource>? GeneratedFiles { get; set; }
        /// <summary>
        /// A token, which can be sent as pageToken to retrieve the next page.
        /// If this field is omitted, there are no more pages.
        /// </summary>
        public string? NextPageToken { get; set; }
    }
}