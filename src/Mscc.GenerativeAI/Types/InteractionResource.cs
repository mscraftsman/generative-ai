namespace Mscc.GenerativeAI
{
    public class InteractionResource
    {
        public int Created { get; set; }
        public string Id { get; set; }
        public string Model { get; set; }
        public string Object { get; set; }
        public Output[] Outputs { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public bool Store { get; set; }
        public int Updated { get; set; }
        public Usage Usage { get; set; }
    }
}

public class Output
{
    public string Thought { get; set; }
    public string ThoughtSignature { get; set; }
    public string Type { get; set; }
    public string Text { get; set; }
}

public class Usage
{
    public int CachedTokens { get; set; }
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    public int ReasoningTokens { get; set; }
    public int ToolUseTokens { get; set; }
    public int TotalTokens { get; set; }
}

