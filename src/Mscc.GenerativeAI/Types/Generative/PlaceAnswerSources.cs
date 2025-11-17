using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The sources that were used to generate the place answer. This includes review snippets and
    /// photos that were used to generate the answer, as well as URIs to flag content. This data type
    /// is not supported in Gemini API.
    /// Collection of sources that provide answers about the features of a given place in Google Maps.
    /// Each PlaceAnswerSources message corresponds to a specific place in Google Maps.
    /// The Google Maps tool used these sources in order to answer questions about features of the
    /// place (e.g: "does Bar Foo have Wifi" or "is Foo Bar wheelchair accessible?").
    /// Currently we only support review snippets as sources.
    /// </summary>
    public class PlaceAnswerSources
    {
        /// <summary>
        /// The place answer is backed by a list of sources like user reviews.
        /// </summary>
        public List<ReviewSnippet>? ReviewSnippets { get; set; }
    }
}