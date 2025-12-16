using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    public partial class BlockedError
    {
        public Candidate? Candidate { get; set; }
        public PromptFeedback? PromptFeedback { get; set; }
    }

    public partial class ResponseBlockedError
    {
        public string Message { get; set; }
        public List<Content> RequestContents { get; set; }
        // type: GenerationResponse
        public List<object> Responses { get; set; }
    }
}
