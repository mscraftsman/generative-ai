namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MaskMode
    {
        /// <summary>
        /// Optional. Prompts the model to generate a mask instead of you needing to provide one. Consequently, when you provide this parameter you can omit a mask object.
        /// </summary>
        /// <remarks>Values:
        /// background: Automatically generates a mask to all regions except primary object, person, or subject in the image
        /// foreground: Automatically generates a mask to the primary object, person, or subject in the image
        /// semantic: Use automatic segmentation to create a mask area for one or more of the segmentation classes. Set the segmentation classes using the classes parameter and the corresponding class_id values. You can specify up to 5 classes.
        /// </remarks>
        public string? MaskType { get; set; }

        /// <summary>
        /// Optional. Determines the classes of objects that will be segmented in an automatically generated mask image.
        /// If you use this field, you must also set "maskType": "semantic".
        /// See <a href="https://cloud.google.com/vertex-ai/generative-ai/docs/model-reference/image-generation#segment-ids">Segmentation class IDs</a>
        /// </summary>
        public int[]? Classes { get; set; }
    }
}