using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    [JsonConverter(typeof(JsonStringEnumConverter<FinishReason>))]

    public enum FinishReason
    {
        /// <summary>
        /// Unspecified means the finish reason is unspecified.
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// Stop means natural stop point of the model or provided stop sequence.
        /// </summary>
        Stop = 1,
        /// <summary>
        /// MaxTokens means the maximum number of tokens as specified in the request was reached.
        /// </summary>
        MaxTokens = 2,
        /// <summary>
        /// Safety means the token generation was stopped as the response was flagged for safety
        /// reasons. NOTE: When streaming the Candidate.content will be empty if
        /// content filters blocked the output.
        /// </summary>
        Safety = 3,
        /// <summary>
        /// Recitation means the token generation was stopped as the response was flagged for
        /// unauthorized citations.
        /// </summary>
        Recitation = 4,
        /// <summary>
        /// Other means all other reasons that stopped the token generation
        /// </summary>
        Other = 5
    }
}