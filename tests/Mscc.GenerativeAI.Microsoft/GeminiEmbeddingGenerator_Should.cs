using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI.Microsoft;
using Neovolve.Logging.Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI.Microsoft
{
	[Collection(nameof(ConfigurationFixture))]
	public class GeminiEmbeddingGenerator_Should : LoggingTestsBase
	{
		private readonly ITestOutputHelper _output;
		private readonly ConfigurationFixture _fixture;
		private readonly string Model = "gemini-embedding-001";

		public GeminiEmbeddingGenerator_Should(ITestOutputHelper output, ConfigurationFixture fixture)
			: base(output, LogLevel.Trace)
		{
			_output = output;
			_fixture = fixture;
		}

		[Fact]
		public async Task GenerateEmbeddingsAsync_String()
		{
			string text = "Some text to get embeddings";
			using var embeddingsGenerator = new GeminiEmbeddingGenerator(apiKey: _fixture.ApiKey, Model, logger: Logger);

			var embeddings =
				await embeddingsGenerator.GenerateAsync(new List<string> { text }, null, CancellationToken.None);

			var result = embeddings.First().Vector.ToArray();
		}
		
		[Fact]
		public async Task GenerateEmbeddingsAsync_IEnumerable()
		{
			IEnumerable<(string text, string? metadata)> items = new (string text, string? metadata)[]
			{
				("one", "m1"),
				("two", "m2"),
				("three", "m3")
			};
			List<(string, string)> list = items != null ? items.ToList<(string, string)>() : throw new ArgumentNullException(nameof (items));
			IEnumerable<string> texts = list.Select<(string, string), string>((Func<(string, string), string>) (i => i.Item1));
			using var embeddingsGenerator = new GeminiEmbeddingGenerator(apiKey: _fixture.ApiKey, Model, logger: Logger);

			var embeddings = await embeddingsGenerator.GenerateAsync(texts, null);

			var result = embeddings.Select(e => e.Vector.ToArray()).ToList();
		}
	}
}