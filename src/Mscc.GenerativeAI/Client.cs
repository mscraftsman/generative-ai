using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI.Types;
using System.Net.Http;

namespace Mscc.GenerativeAI
{
    public sealed class Client : BaseModel
    {
        private readonly IHttpClientFactory? _apiClient;
        private readonly DebugConfig _debugConfig;

        private LiveModel? _live;
        private AioModel? _aio;
        private ModelsModel? _models;
        private TuningsModel? _tunings;
        private CachesModel? _caches;
        private BatchesModel? _batches;
        private FilesModel? _files;
        private AuthTokensModel? _authTokens;
        private OperationsModel? _operations;
        private InteractionsModel? _interactions;
        
        public ChatsModel Chats => new ChatsModel(Models);
        public LiveModel Live => _live ??= new LiveModel(_apiClient, Logger);
        public AioModel Aio => _aio ??= new AioModel(_apiClient, Logger);
        public ModelsModel Models => _models ??= new ModelsModel(_apiClient, Logger);
        public TuningsModel Tunings => _tunings ??= new TuningsModel(_apiClient, Logger);
        public CachesModel Caches => _caches ??= new CachesModel(_apiClient, Logger);
        public BatchesModel Batches => _batches ??= new BatchesModel(_apiClient, Logger);
        public FilesModel Files => _files ??= new FilesModel(_apiClient, Logger);
        public AuthTokensModel AuthTokens => _authTokens ??= new AuthTokensModel(_apiClient, Logger);
        public OperationsModel Operations => _operations ??= new OperationsModel(_apiClient, Logger);
        public InteractionsModel Interactions => _interactions ??= new InteractionsModel(_apiClient, Logger);

        private Client(IHttpClientFactory? httpClientFactory = null, ILogger? logger = null) : base(httpClientFactory, logger)
        {
            _apiVersion = ApiVersion.V1Beta;
            _apiClient = base._httpClientFactory;
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

            ApiKey = apiKey;
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