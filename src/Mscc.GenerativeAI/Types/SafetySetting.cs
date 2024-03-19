using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Safety setting, affecting the safety-blocking behavior.
    /// Ref: https://ai.google.dev/api/rest/v1beta/SafetySetting
    /// </summary>
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class SafetySetting
    {
        /// <summary>
        /// Required. The category for this setting.
        /// </summary>
        public HarmCategory Category { get; set; } = default;
        /// <summary>
        /// Required. Controls the probability threshold at which harm is blocked.
        /// </summary>
        public HarmBlockThreshold Threshold { get; set; } = default;

        private string GetDebuggerDisplay()
        {
            return $"Category: {Category} - Threshold: {Threshold}";
        }
    }
}
