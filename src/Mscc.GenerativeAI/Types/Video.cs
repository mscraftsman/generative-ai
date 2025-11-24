namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// A generated video.
    /// </summary>
    public partial class Video
    {
        /// <summary>
        /// Path to another storage.
        /// </summary>
        public string Uri { get; set; }
        /// <summary>
        /// Video bytes.
        /// </summary>
        public byte[] VideoBytes { get; set; }
        /// <summary>
        /// Video encoding, for example \"video/mp4\".
        /// </summary>
        public string MimeType { get; set; }
    }
}