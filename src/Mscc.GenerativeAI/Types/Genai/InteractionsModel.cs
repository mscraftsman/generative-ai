using System.Threading.Tasks;

namespace Mscc.GenerativeAI
{
    public class InteractionsModel
    {
        public async Task<InteractionResource> Create(string model,
            string input,
            string? systemInstruction = null,
            ThinkingConfig? thinkingConfig = null,
            int? maxOutputTokens = null,
            float? temperature = null,
            float? topP = null,
            int? seed = null,
            string? previousInteractionId = null,
            bool? stream = false,
            bool? background = false)
        {
            
        }

        public async Task<InteractionResource> Get(string generationId = "")
        {
            
        }
    }
}