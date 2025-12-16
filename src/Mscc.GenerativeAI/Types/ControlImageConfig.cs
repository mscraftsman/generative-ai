namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Configuration for a Control reference image.
    /// </summary>
    public partial class ControlImageConfig
    {
        /// <summary>
        /// The type of control reference image to use.
        /// </summary>
        public ControlReferenceType? ControlType { get; set; }
        /// <summary>
        /// When set to True, the control image will be computed by the model based on the control type.
        /// When set to False, the control image must be provided by the user.
        /// </summary>
        /// <remarks>
        /// Defaults to False.
        /// </remarks>
        public bool EnableControlImageComputation { get; set; } = false;
    }
}