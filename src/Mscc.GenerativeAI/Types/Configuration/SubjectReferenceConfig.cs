namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Configuration for a Subject reference image.
    /// </summary>
    public class SubjectReferenceConfig
    {
        /// <summary>
        /// The subject type of a subject reference image.
        /// </summary>
        public SubjectReferenceType? SubjectType { get; set; }
        /// <summary>
        /// Subject description for the image.
        /// </summary>
        public string? SubjectDescription { get; set; }
    }
}