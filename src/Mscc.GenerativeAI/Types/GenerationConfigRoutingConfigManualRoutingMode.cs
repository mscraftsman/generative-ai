namespace Mscc.GenerativeAI
{
    /// <summary>
    /// When manual routing is set, the specified model will be used directly.
    /// </summary>
    public class GenerationConfigRoutingConfigManualRoutingMode
    {
        /// <summary>
        /// The model name to use. Only the public LLM models are accepted. e.g. ‘gemini-1.5-pro-001’.
        /// </summary>
        public string? ModelName { get; set; }
    }
}