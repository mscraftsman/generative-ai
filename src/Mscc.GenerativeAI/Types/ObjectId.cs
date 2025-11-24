namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// This is a copy of the tech.blob.ObjectId proto, which could not be used directly here due to transitive closure issues with JavaScript support; see http://b/8801763.
    /// </summary>
    public partial class ObjectId
    {
        /// <summary>
        /// The name of the object.
        /// </summary>
        public string ObjectName { get; set; }
        /// <summary>
        /// The name of the bucket to which this object belongs.
        /// </summary>
        public string BucketName { get; set; }
        /// <summary>
        /// Generation of the object. Generations are monotonically increasing across writes, allowing them to be be compared to determine which generation is newer. If this is omitted in a request, then you are requesting the live object. See http://go/bigstore-versions
        /// </summary>
        public int Generation { get; set; }
    }
}