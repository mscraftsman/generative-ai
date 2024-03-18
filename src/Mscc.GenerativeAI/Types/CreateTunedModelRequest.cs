namespace Mscc.GenerativeAI
{
    public class CreateTunedModelRequest
    {
        public string DisplayName { get; set; }
        public string BaseModel { get; set; }
        public TuningTask TuningTask { get; set; }
    }
}
