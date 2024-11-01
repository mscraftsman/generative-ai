namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Tool to retrieve public web data for grounding, powered by Google.
    /// </summary>
    public class GoogleSearchRetrieval
    {
        /// <summary>
        /// Specifies the dynamic retrieval configuration for the given source.
        /// </summary>
        public DynamicRetrievalConfig? DynamicRetrievalConfig { get; set; }
        /// <summary>
        /// Optional. Disable using the result from this tool in detecting grounding attribution.
        /// </summary>
        /// <remarks>This does not affect how the result is given to the model for generation.</remarks>
        public bool? DisableAttribution { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="GoogleSearchRetrieval"/>
        /// </summary>
        public GoogleSearchRetrieval() { }
        
        /// <summary>
        /// Creates an instance of <see cref="GoogleSearchRetrieval"/> with Mode and DynamicThreshold.
        /// </summary>
        /// <param name="mode">The mode of the predictor to be used in dynamic retrieval.</param>
        /// <param name="dynamicThreshold">The threshold to be used in dynamic retrieval. If not set, a system default value is used.</param>
        public GoogleSearchRetrieval(DynamicRetrievalConfigMode mode, float dynamicThreshold)
        {
            DynamicRetrievalConfig = new DynamicRetrievalConfig
            {
                Mode = mode, 
                DynamicThreshold = dynamicThreshold
            };
        }
    }
}