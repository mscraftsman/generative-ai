using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class SafetySetting
    {
        /// <summary>
        /// Required. Harm category.
        /// </summary>
        //public HarmCategory Category { get; set; }
        public HarmCategory Category { get; set; } = default;
        /// <summary>
        /// Required. The harm block threshold.
        /// </summary>
        //public HarmBlockThreshold Threshold { get; set; }
        public HarmBlockThreshold Threshold { get; set; } = default;

        private string GetDebuggerDisplay()
        {
            return $"Category: {Category} - Threshold: {Threshold}";
        }
    }
}
