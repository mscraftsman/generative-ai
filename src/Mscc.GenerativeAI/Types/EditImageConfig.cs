namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Edit config object for model versions 006 and greater. All editConfig subfields are optional. If not specified, the default editing mode is inpainting.
    /// </summary>
    public class EditImageConfig : ImageGenerationParameters
    {
        /// <summary>
        /// Optional. Describes the editing mode for the request. One editing mode per request.
        /// </summary>
        public EditMode? EditMode { get; set; }
        
        /// <summary>
        /// Optional. 
        /// </summary>
        public MaskMode? MaskMode { get; set; }

        /// <summary>
        /// Optional. Determines the dilation percentage of the mask provided.
        /// </summary>
        /// <remarks>0.03 (3%) is the default value of shortest side. Minimum: 0, Maximum: 1</remarks>
        public float? MaskDilation { get; set; }

        /// <summary>
        /// Optional. Defines whether the detected product should stay fixed or be repositioned. If you set this field, you must also set "editMode": "product-image".
        /// </summary>
        /// <remarks>Values:
        /// reposition - Lets the model move the location of the detected product or object. (default value)
        /// fixed - The model maintains the original positioning of the detected product or object
        /// If the input image is not square, the model defaults to reposition.
        /// </remarks>
        public string? ProductPosition { get; set; }
    }
}