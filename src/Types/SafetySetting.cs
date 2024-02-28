namespace Mscc.GenerativeAI
{
    public class SafetySetting
    {
        /// <summary>
        /// Required. Harm category.
        /// </summary>
        //public HarmCategory Category { get; set; }
        public string Category { get; set; } = default;
        /// <summary>
        /// Required. The harm block threshold.
        /// </summary>
        //public HarmBlockThreshold Threshold { get; set; }
        public string Threshold { get; set; } = default;
    }
}
