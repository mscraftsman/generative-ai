namespace Mscc.GenerativeAI
{
    public class GenerateAnswerResponse
    {
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
    }
}