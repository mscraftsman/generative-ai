#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Request to generate a grounded answer from the model.
    /// </summary>
    public class GenerateAnswerRequest
    {
        /// <summary>
        /// Required. The content of the current conversation with the model. For single-turn queries, this is a single question to answer. For multi-turn queries, this is a repeated field that contains conversation history and the last Content in the list containing the question.
        /// Note: models.generateAnswer currently only supports queries in English.
        /// </summary>
        public List<Content>? Contents { get; set; }
        /// <summary>
        /// Required. Style in which answers should be returned.
        /// </summary>
        public AnswerStyle AnswerStyle { get; set; }
        /// <summary>
        /// Optional. A list of unique SafetySetting instances for blocking unsafe content.
        /// This will be enforced on the GenerateAnswerRequest.Contents and GenerateAnswerResponse.candidate. There should not be more than one setting for each SafetyCategory type. The API will block any contents and responses that fail to meet the thresholds set by these settings. This list overrides the default settings for each SafetyCategory specified in the safetySettings. If there is no SafetySetting for a given SafetyCategory provided in the list, the API will use the default safety setting for that category. Harm categories HARM_CATEGORY_HATE_SPEECH, HARM_CATEGORY_SEXUALLY_EXPLICIT, HARM_CATEGORY_DANGEROUS_CONTENT, HARM_CATEGORY_HARASSMENT are supported.
        /// </summary>
        public List<SafetySetting>? SafetySettings { get; set; }
        
        /// <summary>
        /// Passages provided inline with the request.
        /// </summary>
        public GroundingPassages? InlinePassages { get; set; }
        /// <summary>
        /// Content retrieved from resources created via the Semantic Retriever API.
        /// </summary>
        public SemanticRetrieverConfig? SemanticRetriever { get; set; }
        /// <summary>
        /// Optional. Controls the randomness of the output.
        /// Values can range from [0.0,1.0], inclusive. A value closer to 1.0 will produce responses that are more varied and creative, while a value closer to 0.0 will typically result in more straightforward responses from the model. A low temperature (~0.2) is usually recommended for Attributed-Question-Answering use cases. 
        /// </summary>
        public float? Temperature { get; set; } = default;
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        public GenerateAnswerRequest() { }

        public GenerateAnswerRequest(string prompt,
            AnswerStyle? answerStyle,
            List<SafetySetting>? safetySettings = null) : this()
        {
            Contents = new List<Content> { new Content 
            {                 
                Parts = new List<IPart> { new TextData
                {
                    Text = prompt
                }}
            }};
            AnswerStyle = answerStyle ?? AnswerStyle.AnswerStyleUnspecified;
            if (safetySettings != null) SafetySettings = safetySettings;
        }
    }

    /// <summary>
    /// Configuration for retrieving grounding content from a Corpus or Document created using the Semantic Retriever API.
    /// </summary>
    public class SemanticRetrieverConfig
    {
        /// <summary>
        /// Required. Name of the resource for retrieval, e.g. corpora/123 or corpora/123/documents/abc.
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// Required. Query to use for similarity matching Chunks in the given resource.
        /// </summary>
        public Content Query { get; set; }
        /// <summary>
        /// Optional. Filters for selecting Documents and/or Chunks from the resource.
        /// </summary>
        public List<MetadataFilter>? MetadataFilters { get; set; }
        /// <summary>
        /// Optional. Maximum number of relevant Chunks to retrieve.
        /// </summary>
        public int? MaxChunkCount { get; set; }
        /// <summary>
        /// Optional. Minimum relevance score for retrieved relevant Chunks.
        /// </summary>
        public float? MinimumRelevanceScore { get; set; }
    }

    /// <summary>
    /// A repeated list of passages.
    /// </summary>
    public class GroundingPassages
    {
        /// <summary>
        /// List of passages.
        /// </summary>
        public List<GroundingPassage> Passages { get; set; }
    }

    /// <summary>
    /// Passage included inline with a grounding configuration.
    /// </summary>
    public class GroundingPassage
    {
        /// <summary>
        /// Identifier for the passage for attributing this passage in grounded answers.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Content of the passage.
        /// </summary>
        public Content Content { get; set; }
    }
}