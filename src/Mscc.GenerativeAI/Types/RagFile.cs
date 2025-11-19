using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class ListRagFilesResponse
    {
        /// <summary>
        /// The list of files.
        /// </summary>
        public List<RagFile> Files { get; set; }
        /// <summary>
        /// A token, which can be sent as pageToken to retrieve the next page.
        /// If this field is omitted, there are no more pages.
        /// </summary>
        public string? NextPageToken { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RagFile
    {
        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}