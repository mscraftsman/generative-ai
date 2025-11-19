using System.Collections.Generic;
using System.Linq;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Generates a response from the model given an input MessagePrompt.
    /// </summary>
    public class GenerateMessageRequest
    {
        /// <summary>
        /// Required. The free-form input text given to the model as a prompt.
        /// Given a prompt, the model will generate a TextCompletion response it predicts as the completion of the input text.
        /// </summary>
        public MessagePrompt Prompt { get; set; }
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
        /// Optional. Number of generated responses to return.
        /// This value must be between [1, 8], inclusive. If unset, this will default to 1.
        /// </summary>
        public int? CandidateCount { get; set; } = default;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GenerateMessageRequest()
        {
            Prompt = new MessagePrompt()
            {
                Messages = new List<Message>()
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prompt"></param>
        public GenerateMessageRequest(string prompt) : this()
        {
            Prompt.Messages.Add(new Message() { Content = prompt });
        }
    }
}