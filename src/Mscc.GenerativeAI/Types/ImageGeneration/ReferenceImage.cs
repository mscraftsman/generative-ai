using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Abstract class that represents a Reference image that is sent to API.
    /// </summary>
    public abstract class ReferenceImage
    {
        /// <summary>
        /// Required. The reference image for the editing operation.
        /// </summary>
        public Image? Image { get; set; }
        /// <summary>
        /// Required. The id of the reference image.
        /// </summary>
        public int ReferenceId { get; set; }
        /// <summary>
        /// Required. The type of the reference image. Only set by the SDK."
        /// </summary>
        public virtual ImageReferenceType ReferenceType { get; set; }
        /// <summary>
        /// Configuration for the mask reference image.
        /// </summary>
        public MaskImageConfig? MaskImageConfig { get; set; }
        /// <summary>
        /// Configuration for the control reference image.
        /// </summary>
        public ControlImageConfig? ControlImageConfig { get; set; }
        /// <summary>
        /// Configuration for the style reference image.
        /// </summary>
        public StyleImageConfig? StyleImageConfig { get; set; }
        /// <summary>
        /// Configuration for the subject reference image.
        /// </summary>
        public SubjectImageConfig? SubjectImageConfig { get; set; }
    }
}