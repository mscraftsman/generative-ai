using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A safety setting that affects the safety-blocking behavior. A SafetySetting consists of a harm
    /// category and a threshold for that category.
    /// </summary>
    /// <remarks>
    /// Represents a safety setting that can be used to control the model's behavior.
    /// It instructs the model to avoid certain responses given safety measurements based on category.
    /// Ref: https://ai.google.dev/api/rest/v1beta/SafetySetting
    /// </remarks>
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class SafetySetting
    {
        /// <summary>
        /// The harm category to be blocked.
        /// </summary>
        public HarmCategory? Category { get; set; }
        /// <summary>
        /// Optional. The method for blocking content. If not specified, the default behavior is to use
        /// the probability score.
        /// </summary>
        /// <remarks>
        /// This field is not supported in Gemini API.
        /// </remarks>
        public HarmBlockMethod? Method { get; set; }
        /// <summary>
        /// The threshold for blocking content. If the harm probability exceeds this threshold, the
        /// content will be blocked.
        /// </summary>
        public HarmBlockThreshold? Threshold { get; set; }

        private string GetDebuggerDisplay()
        {
            return $"Category: {Category} - Threshold: {Threshold}";
        }
    }
}
