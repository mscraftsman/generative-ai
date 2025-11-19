using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A file generated on behalf of a user.
    /// </summary>
    [DebuggerDisplay("{Name} ({State})")]
    public class GeneratedFile
    {
        /// <summary>
        /// Identifier. The name of the generated file. Example: `generatedFiles/abc-123`
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// MIME type of the generatedFile.
        /// </summary>
        public string MimeType { get; set; }
        /// <summary>
        /// Error details if the GeneratedFile ends up in the STATE_FAILED state.
        /// </summary>
        public Status Error { get; set; }
        /// <summary>
        /// Output only. The state of the GeneratedFile.
        /// </summary>
        public StateGeneratedFile State { get; set; }
        
        /// <summary>
        /// The blob reference of the generated file to download.
        /// Only set when the GeneratedFiles.get request url has the \"?alt=media\" query param.
        /// </summary>
        public Media Blob { get; set; }
    }
}