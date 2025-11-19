namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The configuration for automated routing. When automated routing is specified, the routing will
    /// be determined by the pretrained routing model and customer provided model routing preference.
    /// This data type is not supported in Gemini API.
    /// </summary>
    public class AutoRoutingMode
    {
        /// <summary>
        /// The model routing preference.
        /// </summary>
        public string? ModelRoutingPreference { get; set; }
    }
}