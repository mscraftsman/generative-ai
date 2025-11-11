using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Tool to support Google Maps in Model.
    /// </summary>
    public class GoogleMaps
    {
        /// <summary>
        /// Whether to return a token and enable the Google Maps widget (default is false).
        /// </summary>
        public bool? EnableWidget { get; set; }
    }
}