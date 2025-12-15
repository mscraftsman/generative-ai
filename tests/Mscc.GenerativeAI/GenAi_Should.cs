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
            // Act
			var interaction = await client.Interactions.Create(
				model: _model,
				input: "Tell me a short joke about programming."
			);

            // Assert
			_output.WriteLine(interaction.Text);
		}

		[Fact]
		public async Task CreateInteraction_Simple_Request_Streaming()
		{
			// Act
			var stream = client.Interactions.CreateStream(new InteractionRequest
			{
				Model = _model,
				InputString = "Explain quantum entanglement in simple terms."
			});

			// Assert
			stream.Should().NotBeNull();
			await foreach (var chunk in stream)
			{
				if (chunk.EventType == "content.delta")
				{
					if (chunk.Delta.Type == "text")
						_output.WriteLine(chunk.Delta.Text);
					if (chunk.Delta.Type == "thought_signature")	// "thought"
						_output.WriteLine(chunk.Delta.Signature);
				}
				else if (chunk.EventType == "interaction.complete")
				{
					_output.WriteLine($"\n\n--- Stream Finished ---");
					_output.WriteLine($"Total Tokens {chunk.Interaction.Usage.TotalTokens}");
				}
				
			}
		}

		[Fact]
		public async Task CreateInteraction_Simple_Request_with_GenerationConfig()
		{
			// Act
			var interaction = await client.Interactions.Create(
				model: _model,
				input: "Tell me a story about a brave knight.",
				generationConfig: new()
				{
					Temperature = 0.7,
					MaxOutputTokens = 500,
					ThinkingLevel = ThinkingLevel.Low
				}
			);

			// Assert
			_output.WriteLine(interaction.Text);
		}

		[Fact]
		public async Task CreateInteraction_Multi_Turn()
		{
            // Act
			var interaction = await client.Interactions.Create(
				model: _model,
				input: new List<InteractionTurn>
				{
					new() { Role = Role.User, Content = "Hello!" },
					new() { Role = Role.Model, Content = "Hi there! How can I help you today?" },
					new() { Role = Role.User, Content = "What is the capital of France?" }
				}
			);

            // Assert
			_output.WriteLine(interaction.Text);
		}

		[Theory]
		[InlineData("scones.jpg", "image/jpeg", "What is in this picture?", "blueberries")]
		[InlineData("cat.jpg", "image/jpeg", "Describe this image", "snow")]
		public async Task CreateInteraction_Image_Input(string filename, string mimetype, string prompt, string expected)
		{
            // Arrange
			var base64Image =
				Convert.ToBase64String(
					File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "payload", filename)));

            // Act
			var interaction = await client.Interactions.Create(
				model: _model,
				input: new List<InteractionContent>
				{
					new() { Type = "text", Text = prompt },
					new() { Type = "image", Data = base64Image, MimeType = mimetype }
				}
			);

            // Assert
			_output.WriteLine(interaction.Text);
		}

		[Fact]
		public async Task CreateInteraction_Function_Calling()
		{
            // Act
			var interaction = await client.Interactions.Create(
				model: _model,
				input: "What is the weather like in Flic en Flac, Mauritius?"
				// tools: [{
				// 	"type": "function",
				// 	"name": "get_weather",
				// 	"description": "Get the current weather in a given location",
				// 	"parameters": {
				// 	"type": "object",
				// 	"properties": {
				// 	"location": {
				// 	"type": "string",
				// 	"description": "The city and state, e.g. San Francisco, CA"
				// 	}
				// 	},
				// 	"required": ["location"]
				// 	}
				// 	}]
			);

            // Assert
			_output.WriteLine(interaction.Text);
		}

		[Fact]
		public async Task CreateInteraction_Deep_Research()
		{
            // Act
			var initialInteraction = await client.Interactions.Create(
				agent: Model.DeepResearchPro,
				input: "Find a way to clean the oceans from plastic",
				background: true
			);

            // Assert
			_output.WriteLine($"Research started. Interaction ID: {initialInteraction.Id} - {initialInteraction.Status}");

			while (true)
			{
				var interaction = await client.Interactions.Get(initialInteraction.Id);
				_output.WriteLine($"  Status: {interaction.Status}");

				if (interaction.Status == "completed")	// InteractionStatus.Completed
				{
					_output.WriteLine($"\nFinal Report:\n{interaction.Text}");
					break;
				}
				if (interaction.Status == "cancelled" ||
				    interaction.Status == "failed")	// InteractionStatus.Completed
				{
					_output.WriteLine($"Failed with status: {interaction.Status}");
					break;
				}
				
				await Task.Delay(TimeSpan.FromSeconds(10));
			}
		}

		[Fact]
		public async Task Conversation_Stateful()
		{
			// Act
			var interaction1 = await client.Interactions.Create(
				model: _model,
				input: "Hi, my name is JoKi and I'm a Sr software crafter."
			);

			// Assert
			_output.WriteLine(interaction1.Text);

			var interaction2 = await client.Interactions.Create(
				model: _model,
				input: "What's my name?",
				previousInteractionId: interaction1.Id
			);

			// Assert
			_output.WriteLine(interaction2.Text);
		}

		[Fact]
		public async Task Conversation_Stateless()
		{
			// Arrange
			List<InteractionTurn> conversationHistory =
			[
				new() { Role = Role.User, Content = "What are the three largest cities in Germany?" }
			];
			
			// Act
			var interaction1 = await client.Interactions.Create(
				model: _model,
				input: conversationHistory
			);
			
			// Assert
			_output.WriteLine(interaction1.Text);

			conversationHistory.Add(
				new() { Role = Role.Model, Content = interaction1.Outputs });
			conversationHistory.Add(
				new() { Role = Role.User, Content = "What is the most famous landmark in the second one?" });

			var interaction2 = await client.Interactions.Create(
				model: _model,
				input: conversationHistory
			);

			// Assert
			_output.WriteLine(interaction2.Text);
		}

		[Theory]
		[InlineData("image", "scones.jpg", "Describe this image", "blueberries")]
		[InlineData("audio", "pixel.mp3", "What does this audio say?", "Aisha Sherif")]
		[InlineData("video", "Big_Buck_Bunny.mp4", "What is happening in this video? Provide a timestamped summary.", "squirrel")]
		[InlineData("document", "gemini.pdf", "What is this document about?", "DeepMind")]
		public async Task Multimodal_Understanding_Inline(string type, string filename, string prompt, string expected)
		{
			// Arrange
			var mimeType = GenerativeAIExtensions.GetMimeType(filename);
			var base64Image =
				Convert.ToBase64String(
					File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "payload", filename)));

			// Act
			var interaction = await client.Interactions.Create(
				model: _model,
				input: new List<InteractionContent>
				{
					new() { Type = "text", Text = prompt },
					new() { Type = type, Data = base64Image, MimeType = mimeType }
				}
			);

			// Assert
			_output.WriteLine(interaction.Text);
		}

		[Theory]
		[InlineData("image", "image/jpeg", "Describe this image", "blueberries")]
		[InlineData("audio", "audio/mpeg", "What does this audio say?", "Aisha Sherif")]
		[InlineData("video", "video/mp4", "What is happening in this video? Provide a timestamped summary.", "squirrel")]
		[InlineData("document", "application/pdf", "What is this document about?", "DeepMind")]
		public async Task Multimodal_Understanding_using_FilesAPI(string type, string mimeType, string prompt, string expected)
		{
			// Arrange
			var files = await client.Files.ListFiles();
			var file = files.Files
				.FirstOrDefault(x => x.MimeType.StartsWith(mimeType));

			// Act
			var interaction = await client.Interactions.Create(
				model: _model,
				input: new List<InteractionContent>
				{
					new() { Type = "text", Text = prompt },
					new() { Type = type, Uri = file.Uri, MimeType = mimeType }
				}
			);

			// Assert
			_output.WriteLine(interaction.Text);
		}

		[Fact]
		public async Task Multimodal_Generation()
		{
			// Act
			var interaction = await client.Interactions.Create(
				model: Model.Gemini3ProImagePreview,
				input: "Generate an image of a futuristic city.",
				responseModalities: [ResponseModality.Image]
			);

			// Assert
			foreach (InteractionContent output in interaction.Outputs)
			{
				if (output.Type == "image")
				{
					_output.WriteLine($"Generated image with MIME type: {output.MimeType}");
					var fileName = Path.Combine(Environment.CurrentDirectory, "payload",
						Path.ChangeExtension($"{Guid.NewGuid():D}",
							output.MimeType.Replace("image/", "")));
					File.WriteAllBytes(fileName, Convert.FromBase64String(output.Data));
                    _output.WriteLine($"Wrote image to {fileName}");
				}
			}
		}
	}
}