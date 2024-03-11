using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    public class GenerationConfig
    {
        /// <summary>
        /// Optional. Controls the randomness of predictions.
        /// Temperature controls the degree of randomness in token selection. Lower temperatures are good for prompts that expect a true or correct response, while higher temperatures can lead to more diverse or unexpected results. With a temperature of 0, the highest probability token is always selected. 
        /// </summary>
        public float? Temperature { get; set; } = default;
        /// <summary>
        /// Optional. If specified, nucleus sampling will be used.
        /// Top-p changes how the model selects tokens for output. Tokens are selected from most probable to least until the sum of their probabilities equals the top-p value. For example, if tokens A, B and C have a probability of .3, .2 and .1 and the top-p value is .5, then the model will select either A or B as the next token (using temperature).
        /// </summary>
        public float? TopP { get; set; } = default;
        /// <summary>
        /// Optional. If specified, top-k sampling will be used.
        /// Top-k changes how the model selects tokens for output. A top-k of 1 means that the selected token is the most probable among all tokens in the model's vocabulary (also called greedy decoding), while a top-k of 3 means that the next token is selected from among the three most probable tokens (using temperature). 
        /// </summary>
        public int? TopK { get; set; } = default;
        /// <summary>
        /// Optional. Number of candidates to generate.
        /// </summary>
        public int? CandidateCount { get; set; } = default;
        /// <summary>
        /// Optional. The maximum number of output tokens to generate per message.
        /// Token limit determines the maximum amount of text output from one prompt. A token is approximately four characters. 
        /// </summary>
        public int? MaxOutputTokens { get; set; } = default;
        /// <summary>
        /// Optional. Stop sequences.
        /// A stop sequence is a series of characters (including spaces) that stops response generation if the model encounters it. The sequence is not included as part of the response. You can add up to five stop sequences.
        /// </summary>
        public List<string>? StopSequences { get; set; }
    }
}
