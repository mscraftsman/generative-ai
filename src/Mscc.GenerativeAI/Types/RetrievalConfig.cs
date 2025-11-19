namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Retrieval config.
    /// </summary>
    public class RetrievalConfig
    {
        /// <summary>
        /// The location of the user.
        /// </summary>
        public LatLng LatLng { get; set; }
        /// <summary>
        /// The language code of the user.
        /// </summary>
        public string LanguageCode { get; set; }
    }
}