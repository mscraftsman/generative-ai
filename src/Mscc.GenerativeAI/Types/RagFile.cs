using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ListRagFilesResponse
    {
        /// <summary>
        /// The list of files.
        /// </summary>
        public List<RagFile> Files { get; set; }
    }
}