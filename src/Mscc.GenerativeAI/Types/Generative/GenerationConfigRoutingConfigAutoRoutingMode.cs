namespace Mscc.GenerativeAI
{
    /// <summary>
    /// When automated routing is specified, the routing will be determined by the pretrained routing model and customer provided model routing preference.
    /// </summary>
    public class GenerationConfigRoutingConfigAutoRoutingMode
    {
        /// <summary>
        /// The model routing preference.
        /// </summary>
        public ModelRoutingPreference? ModelRoutingPreference { get; set; }
    }
}