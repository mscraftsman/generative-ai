using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Session config for the API connection.
    /// </summary>
    public class LiveConnectConfig
    {
        /// <summary>
        /// The generation configuration for the session.
        /// </summary>
        public GenerateContentConfig? GenerationConfig { get; set; }
        /// <summary>
        /// The requested modalities of the response. Represents the set of modalities that the model can return.
        /// Defaults to AUDIO if not specified.
        /// </summary>
        public List<Modality>? ResponseModalities { get; set; }
        /// <summary>
        /// The speech generation configuration.
        /// </summary>
        public SpeechConfig? SpeechConfig { get; set; }
        /// <summary>
        /// The user provided system instructions for the model.
        /// Note: only text should be used in parts and content in each part will be in a separate paragraph.
        /// </summary>
        public Content? SystemInstruction { get; set; }
        /// <summary>
        /// A list of `Tools` the model may use to generate the next response.
        /// </summary>
        /// <remarks>
        /// A `Tool` is a piece of code that enables the system to interact with external systems to perform an action,
        /// or set of actions, outside of knowledge and scope of the model.
        /// </remarks>
        public Tools? Tools { get; set; }
    }
}