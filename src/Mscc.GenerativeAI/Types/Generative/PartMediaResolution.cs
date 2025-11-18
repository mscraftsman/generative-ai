namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Media resolution for the input media.
    /// </summary>
    public class PartMediaResolution : IPart
    {
        /// <summary>
        /// The tokenization quality used for given media.
        /// </summary>
        public MediaResolution? Level { get; set; }
        
        /// <summary>
        /// Specifies the required sequence length for media tokenization.
        /// </summary>
        public int? NumTokens { get; set; }
    }
}