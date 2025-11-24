namespace Mscc.GenerativeAI.Types
{
    public partial class FileRequest
    {
        /// <summary>
        /// Optional. The human-readable display name for the File. The display name must be no more than 512 characters in length, including spaces. Example: "Welcome Image"
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Optional. The resource name of the File to create.
        /// </summary>
        public string Name { get; set; }
    }
}