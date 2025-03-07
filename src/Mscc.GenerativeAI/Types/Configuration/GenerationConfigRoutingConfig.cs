namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The configuration for routing the request to a specific model.
    /// </summary>
    public class GenerationConfigRoutingConfig
    {
        /// <summary>
        /// Automated routing.
        /// </summary>
        public GenerationConfigRoutingConfigAutoRoutingMode? AutoMode { get; set; }
        /// <summary>
        /// Manual routing.
        /// </summary>
        public GenerationConfigRoutingConfigManualRoutingMode? ManualMode { get; set; }
    }
}