namespace Mscc.GenerativeAI
{
    public static class Method
    {
        // Methods used for models.
        public static string BatchEmbedContents = "batchEmbedContents";
        public static string CountTokens = "countTokens";
        public static string EmbedContent = "embedContent";
        public static string GenerateAnswer = "generateAnswer";
        public static string GenerateContent = "generateContent";
        public static string Get = "get";
        public static string List = "list";
        public static string StreamGenerateContent = "streamGenerateContent";
        public static string BidirectionalGenerateContent = "bidiGenerateContent";

        public static string Predict = "predict";
        public static string PredictLongRunning = "predictLongRunning";

        // Additional methods used for tuned models.
        public static string Create = "create";
        public static string CreateTunedModel = "createTunedModel";
        public static string CreateTunedTextModel = "createTunedTextModel";
        public static string Delete = "delete";
        public static string GenerateText = "generateText";
        public static string Patch = "patch";
        public static string TransferOwnership = "transferOwnership";
        public static string TunedModels = "tunedModels";
        public static string TuningJobs = "tuningJobs";
        
        // Methods for cached content.
        public static string CachedContents = "cachedContents";
        
        // Methods used for PaLM models.
        public static string BatchEmbedText = "batchEmbedText";
        public static string CountMessageTokens = "countMessageTokens";
        public static string CountTextTokens = "countTextTokens";
        public static string EmbedText = "embedText";
        public static string GenerateMessage = "ge0nerateMessage";
    }
}