namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The configuration for routing the request to a specific model. This can be used to control
    /// which model is used for the generation, either automatically or by specifying a model name.
    /// This data type is not supported in Gemini API.
    /// </summary>
    public class RoutingConfig
    {
        /// <summary>
        /// In this mode, the model is selected automatically based on the content of the request.
        /// </summary>
        public AutoRoutingMode? AutoMode { get; set; }
        /// <summary>
        /// In this mode, the model is specified manually.
        /// </summary>
        public ManualRoutingMode? ManualMode { get; set; }
    }
}