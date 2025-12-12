using FluentAssertions;
using Json.Schema.Generation;
using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Types;
using Neovolve.Logging.Xunit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
	[Collection(nameof(ConfigurationFixture))]
	public class GenAiShould : LoggingTestsBase
	{
		private readonly string _model = Model.Gemini25Flash;
		private readonly ITestOutputHelper _output;
		private readonly ConfigurationFixture _fixture;
		private readonly Client client;

		public GenAiShould(ITestOutputHelper output, ConfigurationFixture fixture)
			: base(output, LogLevel.Trace)
		{
			_output = output;
			_fixture = fixture;
			client = new Client(apiKey: _fixture.ApiKey, logger: Logger);
		}

		[Fact]
		public async Task CreateInteraction_Simple_Request()
		{
			var interaction = await client.Interactions.Create(
				model: _model,
				input: "Hello, how are you?"
			);

			_output.WriteLine(interaction.Text);
		}

		[Fact]
		public async Task CreateInteraction_Multi_Turn()
		{
			var interaction = await client.Interactions.Create(
				model: _model,
				input: new List<InteractionTurn>
				{
					new() { Role = Role.User, Content = [new() { Text = "Hello!" }] },
					new() { Role = Role.Model, Content = [new() { Text = "Hi there! How can I help you today?" }] },
					new() { Role = Role.User, Content = [new() { Text = "What is the capital of France?" }] }
				}
			);

			_output.WriteLine(interaction.Text);
		}

		[Fact]
		public async Task CreateInteraction_Image_Input()
		{
			var interaction = await client.Interactions.Create(
				model: _model,
				input: new List<InteractionContent>
				{
					new() { Type = "text", Text = "Hello!" },
					new() { Type = "image", Data = "", MimeType = "image/png" }
				}
			);

			_output.WriteLine(interaction.Text);
		}

		[Fact]
		public async Task CreateInteraction_Function_Calling()
		{
			var interaction = await client.Interactions.Create(
				model: _model,
				input: "What is the weather like in Flic en Flac, Mauritius?",
				tools: []
			);

			_output.WriteLine(interaction.Text);
		}

		[Fact]
		public async Task CreateInteraction_Deep_Research()
		{
			var interaction = await client.Interactions.Create(
				agent: Model.DeepResearchPro,
				input: "Find a way to clean the oceans from plastic",
				background: true
			);

			_output.WriteLine(interaction.Status);
		}
	}
}