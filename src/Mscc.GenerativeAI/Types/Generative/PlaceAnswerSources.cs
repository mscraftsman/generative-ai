#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Place answers provide deeper contextual information about a specific place using data, such as user reviews.
    /// </summary>
    public class PlaceAnswerSources
    {
        /// <summary>
        /// The place answer is backed by a list of sources like user reviews.
        /// </summary>
        public List<ReviewSnippet>? ReviewSnippets { get; set; }
    }
}