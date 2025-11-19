namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The configuration for manual routing. When manual routing is specified, the model will be
    /// /// selected based on the model name provided. This data type is not supported in Gemini API.
    /// </summary>
    public class ManualRoutingMode
    {
        /// <summary>
        /// The name of the model to use. Only public LLM models are accepted.
        /// </summary>
        public string? ModelName { get; set; }
    }
}