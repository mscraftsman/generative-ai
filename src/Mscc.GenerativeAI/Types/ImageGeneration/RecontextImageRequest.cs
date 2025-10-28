namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Request for recontextualizing an image.
    /// </summary>
    public class RecontextImageRequest
    {
        /// <summary>
        /// ID of the model to use. For a list of models, see Google models
        /// (https://cloud.google.com/vertex-ai/generative-ai/docs/learn/models).
        /// </summary>
        public string? Model { get; set; }
        /// <summary>
        /// A set of source input(s) for image recontextualization.
        /// </summary>
        public RecontextImageSource? Source { get; set; }
        /// <summary>
        /// Configuration for image recontextualization.
        /// </summary>
        public RecontextImageConfig? Config { get; set; }
    }
}