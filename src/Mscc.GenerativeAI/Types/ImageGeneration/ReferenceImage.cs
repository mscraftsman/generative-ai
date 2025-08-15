namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Abstract class that represents a Reference image that is sent to API.
    /// </summary>
    public abstract class ReferenceImage
    {
        /// <summary>
        /// The reference image for the editing operation.
        /// </summary>
        public Image? Image { get; set; }
        /// <summary>
        /// The id of the reference image.
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// The type of the reference image. Only set by the SDK."
        /// </summary>
        public ReferenceType? ReferenceType { get; set; }
        /// <summary>
        /// Configuration for the mask reference image.
        /// </summary>
        public MaskReferenceConfig? MaskImageConfig { get; set; }
        /// <summary>
        /// Configuration for the control reference image.
        /// </summary>
        public ControlReferenceConfig? ControlImageConfig { get; set; }
        /// <summary>
        /// Configuration for the style reference image.
        /// </summary>
        public StyleReferenceConfig? StyleImageConfig { get; set; }
        /// <summary>
        /// Configuration for the subject reference image.
        /// </summary>
        public SubjectReferenceConfig? SubjectImageConfig { get; set; }
    }
}