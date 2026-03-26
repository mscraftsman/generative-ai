using System;

namespace Mscc.GenerativeAI
{
    internal static class Transformers
    {
        public static string? TModel(BaseModel apiClient, string model)
        {
            if (string.IsNullOrEmpty(model)) return model;
            if (model.StartsWith("models/")) return model;
            return $"models/{model}";
        }
    }
}
