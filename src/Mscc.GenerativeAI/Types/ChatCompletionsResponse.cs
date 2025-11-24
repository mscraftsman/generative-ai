namespace Mscc.GenerativeAI.Types
{
    public partial class ChatCompletionsResponse
    {
        public string Object { get; set; }
        public int Created { get; set; }
        public string Model { get; set; }
        public Choices[] Choices { get; set; }
    }

    public partial class Choices
    {
        public int Index { get; set; }
        public ChatMessage Message { get; set; }
        public string FinishReason { get; set; }
    }

    public partial class ChatMessage
    {
        public string Content { get; set; }
        public string Role { get; set; }
        public object[] ToolCalls { get; set; }
    }
}