namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Tool to retrieve public web data for grounding, powered by Google.
    /// </summary>
    public class GoogleSearchRetrieval
    {
        /// <summary>
        /// Optional. Disable using the result from this tool in detecting grounding attribution.
        /// </summary>
        /// <remarks>This does not affect how the result is given to the model for generation.</remarks>
        public bool? DisableAttribution { get; set; }
    }
}