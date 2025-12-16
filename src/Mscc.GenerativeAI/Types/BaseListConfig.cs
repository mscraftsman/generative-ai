namespace Mscc.GenerativeAI.Types
{
    public partial class BaseListConfig
    {
        public int PageSize { get; set; } = 50;
        public string? PageToken { get; set; }
    }
}