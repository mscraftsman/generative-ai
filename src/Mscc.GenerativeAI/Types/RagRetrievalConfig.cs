namespace Mscc.GenerativeAI
{
    public class RagRetrievalConfig
    {
        /// <summary>
        /// Optional. If specified, top-k sampling will be used.
        /// Top-k changes how the model selects tokens for output. A top-k of 1 means that the selected token is the most probable among all tokens in the model's vocabulary (also called greedy decoding), while a top-k of 3 means that the next token is selected from among the three most probable tokens (using temperature). 
        /// </summary>
        public int? TopK { get; set; } = default;

        public HybridSearch? HybridSearch { get; set; }

        /// <summary>
        /// Optional. Config for filters.
        /// </summary>
        public Filter Filter { get; set; }
        public Ranking Ranking { get; set; }
    }
}