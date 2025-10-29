#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif
using System.Drawing;
using mea = Microsoft.Extensions.AI;

namespace Mscc.GenerativeAI.Microsoft
{
    public sealed class GeminiImageGenerator : mea.IImageGenerator
    {
        private const string ProviderName = "gemini";
        private const string BaseUrl = "https://generativelanguage.googleapis.com/";

        /// <summary>
        /// Gets the Gemini model that is used to communicate with.
        /// </summary>
        private readonly GenerativeModel _client;
        /// <summary>Lazily-initialized metadata describing the implementation.</summary>
        private mea.ImageGeneratorMetadata? _metadata;
        /// <summary>The default model that should be used when no override is specified.</summary>
        private readonly string? _model;

        /// <summary>
        /// Creates a new instance of the <see cref="GeminiImageGenerator"/> class for the specified Gemini API client.
        /// </summary>
        /// <param name="client">The underlying client.</param>
        /// <exception cref="ArgumentNullException">Thrown when the specified client is null.</exception>
        public GeminiImageGenerator(GenerativeModel client, string? model)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _model = model ?? client.Model;
        }

        /// <summary>
        /// Creates an instance of the Gemini API client using Google AI.
        /// </summary>
        /// <param name="apiKey">API key provided by Google AI Studio</param>
        /// <param name="model">Model to use.</param>
        public GeminiImageGenerator(string apiKey, string? model)
        {
            var genAi = new GoogleAI(apiKey);
            _client = genAi.GenerativeModel(model);
            _model = model ?? _client.Model;
        }

        /// <summary>
        /// Creates an instance of the Gemini API client using Vertex AI.
        /// </summary>
        /// <param name="projectId">Identifier of the Google Cloud project.</param>
        /// <param name="region">Optional. Region to use (default: "us-central1").</param>
        /// <param name="model">Model to use.</param>
        public GeminiImageGenerator(string projectId, string? region = null, string? model = null)
        {
            var genAi = new VertexAI(projectId: projectId, region: region);
            _client = genAi.GenerativeModel(model);
            _model = model ?? _client.Model;
        }

        /// <inheritdoc/>
        public async Task<mea.ImageGenerationResponse> GenerateAsync(mea.ImageGenerationRequest request, 
            mea.ImageGenerationOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            string modelId = options?.ModelId ?? _model ?? "";
            string prompt = request.Prompt ?? "";

            GenerateImagesConfig config = options?.RawRepresentationFactory?.Invoke(this) as GenerateImagesConfig ??
                                          new();
            config.NumberOfImages ??= options?.Count ?? 1;
            config.OutputMimeType ??= options?.MediaType;
            config.AspectRatio ??= options?.ImageSize is { } imageSize && imageSize.Width > 0 && imageSize.Height > 0
                ? GetClosestAspectRatio(imageSize)
                : null;

            var gir = await _client.GenerateImages(model: modelId, 
                prompt: prompt, 
                config: config, 
                cancellationToken: cancellationToken).ConfigureAwait(false);

            mea.ImageGenerationResponse response = new() { RawRepresentation = gir };

            if (gir.GeneratedImages is { } generatedImages)
            {
                foreach (var generatedImage in generatedImages)
                {
                    if (generatedImage.Image is { } image)
                    {
                        if (!string.IsNullOrEmpty(image.GcsUri))
                        {
                            response.Contents.Add(new mea.UriContent(image.GcsUri, image.MimeType ?? "image/*"));
                        }
                        else if (image.ImageBytes is { } bytes)
                        {
                            response.Contents.Add(new mea.DataContent(bytes, image.MimeType ?? "image/*"));
                        }
                    }
                }
            }

            return response;
        }
        
        /// <inheritdoc />
        object? mea.IImageGenerator.GetService(Type serviceType, object? serviceKey) =>
            serviceKey is not null ? null :
            serviceType == typeof(mea.ImageGeneratorMetadata) ? _metadata ?? new(ProviderName, new(BaseUrl), _model) :
            serviceType == typeof(GenerativeModel) ? _client :
            serviceType?.IsInstanceOfType(this) is true ? this :
            null;

        /// <inheritdoc />
        public void Dispose() { }

        private static ImageAspectRatio GetClosestAspectRatio(Size size)
        {
            float ratio = (float)size.Width / size.Height;

            float closestDifference = float.MaxValue;
            ImageAspectRatio result = ImageAspectRatio.Ratio1x1;

            foreach ((ImageAspectRatio aspectRatio, float knownAspectRatio) in s_knownAspectRatios)
            {
                float difference = Math.Abs(knownAspectRatio - ratio);
                if (difference < closestDifference)
                {
                    closestDifference = difference;
                    result = aspectRatio;
                }
            }

            return result;
        }

        private static readonly (ImageAspectRatio AspectRatio, float Ratio)[] s_knownAspectRatios = new[]
        {
            (ImageAspectRatio.Ratio1x1, 1f),
            (ImageAspectRatio.Ratio2x3, 2f / 3f),
            (ImageAspectRatio.Ratio3x2, 3f / 2f),
            (ImageAspectRatio.Ratio3x4, 3f / 4f),
            (ImageAspectRatio.Ratio4x3, 4f / 3f),
            (ImageAspectRatio.Ratio9x16, 9f / 16f),
            (ImageAspectRatio.Ratio16x9, 16f / 9f),
            (ImageAspectRatio.Ratio21x9, 21f / 9f)
        };
    }
}