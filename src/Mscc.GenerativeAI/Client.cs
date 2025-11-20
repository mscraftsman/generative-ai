using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI.Types;
using System.Net.Http;

namespace Mscc.GenerativeAI
{
    public sealed class Client : BaseModel
    {
        private readonly IHttpClientFactory? _apiClient;
        private readonly DebugConfig _debugConfig;

        private readonly LiveModel _live;
        private readonly AioModel _aio;
        private readonly ModelsModel _models;
        private readonly TuningsModel _tunings;
        private readonly CachesModel _caches;
        private readonly BatchesModel _batches;
        private readonly FilesModel _files;
        private readonly AuthTokensModel _authTokens;
        private readonly OperationsModel _operations;

        private readonly InteractionsModel _interactions;
        
        public ChatsModel Chats => new ChatsModel(_models);
        public LiveModel Live => _live ?? new LiveModel(_apiClient);
        public AioModel Aio => _aio ?? new AioModel(_apiClient);
        public ModelsModel Models => _models ?? new ModelsModel(_apiClient);
        public TuningsModel Tunings => _tunings ?? new TuningsModel(_apiClient);
        public CachesModel Caches => _caches ?? new CachesModel(_apiClient);
        public BatchesModel Batches => _batches ?? new BatchesModel(_apiClient);
        public FilesModel Files => _files ?? new FilesModel(_apiClient);
        public AuthTokensModel AuthTokens => _authTokens ?? new AuthTokensModel(_apiClient);
        public OperationsModel Operations => _operations ?? new OperationsModel(_apiClient);

        public InteractionsModel Interactions => _interactions ?? new InteractionsModel();
        
        public Client(IHttpClientFactory? httpClientFactory = null, ILogger? logger = null) : base(httpClientFactory, logger)
        {
            _apiVersion = ApiVersion.V1Beta;
            Logger.LogClientInvoking();
        }
        
        public Client(bool? vertexai = null,
            string? apiKey = null,
            Credentials? credentials = null,
            string? project = null,
            string? location = null,
            DebugConfig? debugConfig = null,
            RequestOptions? httpOptions = null,
            IHttpClientFactory? httpClientFactory = null, 
            ILogger? logger = null) : this(httpClientFactory, logger)
        {
            _debugConfig = debugConfig ?? new DebugConfig();
            var baseUrl = GetBaseUrl(vertexai ?? false, httpOptions);
            if (httpOptions is not null)
            {
                httpOptions.BaseUrl = baseUrl;
            }
            else
            {
                httpOptions = new RequestOptions() { BaseUrl = baseUrl };
            }
            
#if false
            _apiClient = GetApiClient(vertexai,
                apiKey,
                credentials,
                project,
                location,
                _debugConfig,
                httpOptions);
#endif
        }

#if false
        private IHttpClientFactory GetApiClient(bool? vertexai,
            string? apiKey,
            Credentials? credentials,
            string? project,
            string? location,
            DebugConfig? debugConfig,
            RequestOptions? httpOptions)
        {
            if (debugConfig?.ClientMode == "auto")
                return ReplayApiClient();

            return BaseApiClient(vertexai,
                apiKey,
                credentials,
                project,
                location,
                httpOptions);
        }
#endif
	    
        private string GetBaseUrl(bool vertexai, RequestOptions httpOptions)
        {
            if (!string.IsNullOrEmpty(httpOptions?.BaseUrl)) return httpOptions.BaseUrl;

            if (vertexai)
            {
                return string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}