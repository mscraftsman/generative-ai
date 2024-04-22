using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Safety setting, affecting the safety-blocking behavior.
    /// Represents a safety setting that can be used to control the model's behavior.
    /// It instructs the model to avoid certain responses given safety measurements based on category.
    /// Ref: https://ai.google.dev/api/rest/v1beta/SafetySetting
    /// </summary>
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class SafetySetting
    {
        /// <summary>
        /// Required. The category for this setting.
        /// </summary>
        public HarmCategory Category { get; set; }
        /// <summary>
        /// Required. Controls the probability threshold at which harm is blocked.
        /// </summary>
        public HarmBlockThreshold Threshold { get; set; }

        private string GetDebuggerDisplay()
        {
            return $"Category: {Category} - Threshold: {Threshold}";
        }
    }
}
