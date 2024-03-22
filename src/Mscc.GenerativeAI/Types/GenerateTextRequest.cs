#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
using System.Linq;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class GenerateTextRequest
    {
        /// <summary>
        /// Required. The free-form input text given to the model as a prompt.
        /// Given a prompt, the model will generate a TextCompletion response it predicts as the completion of the input text.
        /// </summary>
        public TextPrompt Prompt { get; set; }
        /// <summary>
        /// Optional. A list of unique SafetySetting instances for blocking unsafe content.
        /// This will be enforced on the GenerateContentRequest.contents and GenerateContentResponse.candidates. There should not be more than one setting for each SafetyCategory type. The API will block any contents and responses that fail to meet the thresholds set by these settings. This list overrides the default settings for each SafetyCategory specified in the safetySettings. If there is no SafetySetting for a given SafetyCategory provided in the list, the API will use the default safety setting for that category. Harm categories HARM_CATEGORY_HATE_SPEECH, HARM_CATEGORY_SEXUALLY_EXPLICIT, HARM_CATEGORY_DANGEROUS_CONTENT, HARM_CATEGORY_HARASSMENT are supported.
        /// </summary>
        public List<SafetySetting>? SafetySettings { get; set; }
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
        /// Optional. The maximum number of output tokens to generate per message.
        /// Token limit determines the maximum amount of text output from one prompt. A token is approximately four characters. 
        /// </summary>
        public int? MaxOutputTokens { get; set; } = default;
        /// <summary>
        /// Optional. Stop sequences.
        /// A stop sequence is a series of characters (including spaces) that stops response generation if the model encounters it. The sequence is not included as part of the response. You can add up to five stop sequences.
        /// </summary>
        public List<string>? StopSequences { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GenerateTextRequest() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prompt"></param>
        public GenerateTextRequest(string prompt) : this()
        {
            Prompt = new TextPrompt() { Text = prompt };
        }
    }
}