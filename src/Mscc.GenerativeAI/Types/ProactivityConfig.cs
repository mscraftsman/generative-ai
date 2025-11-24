namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Config for proactivity features.
    /// </summary>
    public partial class ProactivityConfig
    {
        /// <summary>
        /// Optional. If enabled, the model can reject responding to the last prompt.
        /// For example, this allows the model to ignore out of context speech or to stay silent
        /// if the user did not make a request, yet.
        /// </summary>
        public bool? ProactiveAudio { get; set; }
    }
}