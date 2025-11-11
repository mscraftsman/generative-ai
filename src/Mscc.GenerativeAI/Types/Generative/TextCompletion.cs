using System.Collections.Generic;
using System.Linq;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Output text returned from a model.
    /// </summary>
    public sealed class TextCompletion
    {
        /// <summary>
        /// Output only. The generated text returned from the model.
        /// </summary>
        public string Output { get; set; }
        /// <summary>
        /// Ratings for the safety of a response.
        /// There is at most one rating per category.
        /// </summary>
        public List<SafetyRating> SafetyRatings { get; set; }
        /// <summary>
        /// Output only. Citation information for model-generated output in this TextCompletion.
        /// This field may be populated with attribution information for any text included in the output.
        /// </summary>
        public CitationMetadata CitationMetadata { get; set; }
    }
}