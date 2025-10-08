namespace Mscc.GenerativeAI
{
    public static class Method
    {
        // Methods used for models.
        public const string AsyncBatchEmbedContent = "asyncBatchEmbedContent";
        public const string BatchEmbedContents = "batchEmbedContents";
        public const string BatchEmbedText = "batchEmbedText";
        public const string BatchGenerateContent = "batchGenerateContent";
        public const string CountTokens = "countTokens";
        public const string ComputeTokens = "computeTokens";
        public const string EmbedContent = "embedContent";
        public const string GenerateAnswer = "generateAnswer";
        public const string GenerateContent = "generateContent";
        public const string Get = "get";
        public const string List = "list";
        public const string Copy = "copy";
        public const string StreamGenerateContent = "streamGenerateContent";
        public const string BidirectionalGenerateContent = "bidiGenerateContent";

        public const string Generate = "generate";

        public const string Predict = "predict";
        public const string PredictLongRunning = "predictLongRunning";
        public const string RawPredict = "rawPredict";
        public const string StreamRawPredict = "streamRawPredict";
        public const string FetchPredictOperation = "fetchPredictOperation";

        public const string Query = "query";
        public const string RetrieveContexts = "retrieveContexts";

        // Additional methods used for tuned models.
        public const string Create = "create";
        public const string CreateTunedModel = "createTunedModel";
        public const string CreateTunedTextModel = "createTunedTextModel";
        public const string Delete = "delete";
        public const string GenerateText = "generateText";
        public const string Patch = "patch";
        public const string TransferOwnership = "transferOwnership";
        public const string TunedModels = "tunedModels";
        public const string TuningJobs = "tuningJobs";
        
        // Methods for cached content.
        public const string CachedContents = "cachedContents";
        
        // Methods used for PaLM models.
        public const string CountMessageTokens = "countMessageTokens";
        public const string CountTextTokens = "countTextTokens";
        public const string EmbedText = "embedText";
        public const string GenerateMessage = "generateMessage";
    }
}