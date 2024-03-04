using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    [JsonConverter(typeof(JsonStringEnumConverter<FinishReason>))]

    public enum FinishReason
    {
        /// <summary>
        /// FinishReasonUnspecified means the finish reason is unspecified.
        /// </summary>
        FinishReasonUnspecified = 0,
        /// <summary>
        /// FinishReasonStop means natural stop point of the model or provided stop sequence.
        /// </summary>
        FinishReasonStop = 1,
        /// <summary>
        /// FinishReasonMaxTokens means the maximum number of tokens as specified in the request was reached.
        /// </summary>
        FinishReasonMaxTokens = 2,
        /// <summary>
        /// FinishReasonSafety means the token generation was stopped as the response was flagged for safety
        /// reasons. NOTE: When streaming the Candidate.content will be empty if
        /// content filters blocked the output.
        /// </summary>
        FinishReasonSafety = 3,
        /// <summary>
        /// FinishReasonRecitation means the token generation was stopped as the response was flagged for
        /// unauthorized citations.
        /// </summary>
        FinishReasonRecitation = 4,
        /// <summary>
        /// FinishReasonOther means all other reasons that stopped the token generation
        /// </summary>
        FinishReasonOther = 5
    }
}