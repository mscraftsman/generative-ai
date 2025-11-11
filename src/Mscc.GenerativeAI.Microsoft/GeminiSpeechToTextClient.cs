using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using mea = Microsoft.Extensions.AI;

namespace Mscc.GenerativeAI.Microsoft
{
    public sealed class GeminiSpeechToTextClient : mea.ISpeechToTextClient
    {
        private const string ProviderName = "gemini";

        /// <summary>
        /// Gets the Gemini model that is used to communicate with.
        /// </summary>
        private readonly GenerativeModel _client;

        private readonly mea.SpeechToTextClientMetadata _metadata;

        /// <summary>
        /// Creates an instance of the <see cref="GeminiSpeechToTextClient"/> class for the specified Gemini API client.
        /// </summary>
        /// <param name="client">The underlying client.</param>
        /// <exception cref="ArgumentNullException">Thrown when the specified client is null.</exception>
        public GeminiSpeechToTextClient(GenerativeModel client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _metadata = new(ProviderName, null, client.Model);
        }

        /// <inheritdoc/>
        public async Task<mea.SpeechToTextResponse> GetTextAsync(
            Stream audioSpeechStream,
            mea.SpeechToTextOptions? options = null,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (audioSpeechStream == null) throw new ArgumentNullException(nameof(audioSpeechStream));

            mea.SpeechToTextResponse response = new();

            static bool IsTranslationRequest(mea.SpeechToTextOptions? options)
                => options?.TextLanguage != null
                   && (options.SpeechLanguage is null ||
                       options.SpeechLanguage != options.TextLanguage);

            if (IsTranslationRequest(options))
            {
                if (options == null) throw new ArgumentNullException(nameof(options));

#if NET
                await using (audioSpeechStream.ConfigureAwait(false))
#else
                using (audioSpeechStream)
#endif
                {
                    // TODO: Implement API call in underlying GenerativeModel.
                    throw new NotImplementedException();
                }
            }
            else
            {
#if NET
                await using (audioSpeechStream.ConfigureAwait(false))
#else
                using (audioSpeechStream)
#endif
                {
                    // TODO: Implement API call in underlying GenerativeModel.
                    throw new NotImplementedException();
                }
            }

            return response;
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<mea.SpeechToTextResponseUpdate> GetStreamingTextAsync(
            Stream audioSpeechStream,
            mea.SpeechToTextOptions? options = null,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (audioSpeechStream == null) throw new ArgumentNullException(nameof(audioSpeechStream));

            var speechResponse =
                await GetTextAsync(audioSpeechStream, options, cancellationToken).ConfigureAwait(false);

            foreach (var update in speechResponse.ToSpeechToTextResponseUpdates())
            {
                yield return update;
            }
        }

        /// <inheritdoc/>
        public object? GetService(Type serviceType, object? serviceKey = null) =>
            serviceKey is not null ? null :
            serviceType == typeof(mea.SpeechToTextClientMetadata) ? _metadata :
            serviceType == typeof(GenerativeModel) ? _client :
            serviceType?.IsInstanceOfType(this) is true ? this :
            null;

        /// <inheritdoc/>
        public void Dispose() { }
    }
}