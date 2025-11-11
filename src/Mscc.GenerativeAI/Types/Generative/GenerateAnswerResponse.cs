using System;
using System.Collections.Generic;
using System.Linq;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Response from the model for a grounded answer.
    /// </summary>
    public class GenerateAnswerResponse
    {
        /// <summary>
        /// Responded text information of first candidate.
        /// </summary>
        public string? Text
        {
            get
            {
                if (Answer?.FinishReason == FinishReason.Safety)
                    return string.Empty;
                return Answer?.Content?.Parts?.FirstOrDefault()?.Text;
            }
        }

        /// <summary>
        /// Candidate answer from the model.
        /// Note: The model always attempts to provide a grounded answer, even when the answer is unlikely to be answerable from the given passages. In that case, a low-quality or ungrounded answer may be provided, along with a low answerableProbability.
        /// </summary>
        public Candidate Answer { get; set; }
        /// <summary>
        /// Output only. The model's estimate of the probability that its answer is correct and grounded in the input passages.
        /// A low answerableProbability indicates that the answer might not be grounded in the sources.
        /// </summary>
        public float AnswerableProbability { get; set; }
        /// <summary>
        /// Output only. Feedback related to the input data used to answer the question, as opposed to model-generated response to the question.
        /// </summary>
        public PromptFeedback InputFeedback { get; set; }

        /// <summary>
        /// A convenience overload to easily access the responded text.
        /// </summary>
        /// <returns>The responded text information of first candidate.</returns>
        public override string ToString()
        {
            return Text ?? String.Empty;
        }
    }
}