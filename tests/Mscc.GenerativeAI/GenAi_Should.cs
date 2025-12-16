using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Types;
using Neovolve.Logging.Xunit;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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
			client = new Client(logger: Logger);
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
			stream.ShouldNotBeNull();
			await foreach (var chunk in stream)
			{
				if (chunk.EventType == "content.delta")
				{
					if (chunk.Delta.Type == InteractionContentType.Text)
						_output.WriteLine(chunk.Delta.Text);
					if (chunk.Delta.Type == InteractionContentType.ThoughtSignature)	// "thought"
						_output.WriteLine($"Signature: {chunk.Delta.Signature}\n");
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
					new() { Type = InteractionContentType.Text, Text = prompt },
					new() { Type = InteractionContentType.Image, Data = base64Image, MimeType = mimetype }
				}
			);

            // Assert
			_output.WriteLine(interaction.Text);
		}

		string GetCurrentWeather(string location)
		{
			return $"The weather in {location} is sunny.";
		}

		[Fact]
		public async Task Tools_Use_Function_Calling()
		{
			// Arrange
			var tools = new InteractionTools();
			tools.Add(GetCurrentWeather, null, "Get the current weather in a given location.");
			
            // Act
			var interaction = await client.Interactions.Create(
				model: _model,
				input: "What is the weather like in Flic en Flac, Mauritius?",
				tools: tools
			);

			foreach (var output in interaction.Outputs)
			{
				if (output.Type == InteractionContentType.FunctionCall)
				{
					_output.WriteLine($"Function Call: {output.Name}({output.Arguments})");
					var result = tools.Invoke(output.Name, output.Arguments);

					interaction = await client.Interactions.Create(
						model: _model,
						previousInteractionId: interaction.Id,
						input: new List<InteractionContent>
						{
							new()
							{
								Type = InteractionContentType.FunctionResult,
								Name = output.Name,
								CallId = output.Id,
								Result = result
							}
						}
					);
				}
			}

			// Assert
			_output.WriteLine(interaction.Text);
		}

		[Fact]
		public async Task Tools_Use_Google_Search()
		{
			// Arrange

			// Act
			var interaction = await client.Interactions.Create(
				model: _model,
				input: "Who is the current prime minister in Mauritius?",
				tools: [new InteractionTool { Type = InteractionToolType.GoogleSearch }]
			);

			// Assert
			_output.WriteLine(interaction.Text);
		}

		[Fact]
		public async Task Tools_Use_Code_Execution()
		{
			// Arrange

			// Act
			var interaction = await client.Interactions.Create(
				model: _model,
				input: "Calculate the first 10 Fibonacci numbers",
				tools: [new InteractionTool { Type = InteractionToolType.CodeExecution }]
			);

			// Assert
			_output.WriteLine(interaction.Text);
		}

		[Fact]
		public async Task Tools_Use_Url_Context()
		{
			// Arrange

			// Act
			var interaction = await client.Interactions.Create(
				model: _model,
				input: "Summarize https://jochen.kirstaetter.name/",
				tools: [new InteractionTool { Type = InteractionToolType.UrlContext }]
			);

			// Assert
			_output.WriteLine(interaction.Text);
		}

		[Fact]
		public async Task Tools_Use_Computer_Use()
		{
			// Arrange

			// Act
			var interaction = await client.Interactions.Create(
				model: Model.Gemini25ComputerUse,
				input: "Find a flight to Tokyo",
				tools: [new InteractionTool { Type = InteractionToolType.ComputerUse }]
			);

			// Assert
			_output.WriteLine(interaction.Text);
		}

		[Fact]
		public async Task Tools_Use_Mcp_Server()
		{
			// Arrange

			// Act
			var interaction = await client.Interactions.Create(
				model: _model,
				input: $"Today is {DateTime.Now.Date}. What is the temperature today in Flic en Flac, Mauritius?",
				tools: [new InteractionTool
					{
						Type = InteractionToolType.McpServer,
						Name = "weather_service",
						Url = "https://gemini-api-demos.uc.r.appspot.com/mcp"
					}
				]
			);

			// Assert
			_output.WriteLine(interaction.Text);
		}

		[Fact]
		public async Task Tools_Use_File_Search()
		{
			// Arrange

			// Act
			var interaction = await client.Interactions.Create(
				model: _model,
				input: "Who is the author of the book?",
				tools: [new InteractionTool
				{
					Type = InteractionToolType.FileSearch,
					FileSearchStoreNames = ["fileSearchStores/m64d1sevsr4y-xfyawui3fxqg"]
				}]
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
		[InlineData(InteractionContentType.Image, "scones.jpg", "Describe this image", "blueberries")]
		[InlineData(InteractionContentType.Audio, "pixel.mp3", "What does this audio say?", "Aisha Sherif")]
		[InlineData(InteractionContentType.Video, "Big_Buck_Bunny.mp4", "What is happening in this video? Provide a timestamped summary.", "squirrel")]
		[InlineData(InteractionContentType.Document, "gemini.pdf", "What is this document about?", "DeepMind")]
		public async Task Multimodal_Understanding_Inline(InteractionContentType type, string filename, string prompt, string expected)
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
					new() { Type = InteractionContentType.Text, Text = prompt },
					new() { Type = type, Data = base64Image, MimeType = mimeType }
				}
			);

			// Assert
			_output.WriteLine(interaction.Text);
		}

		[Theory]
		[InlineData(InteractionContentType.Image, "image/jpeg", "Describe this image", "blueberries")]
		[InlineData(InteractionContentType.Audio, "audio/mpeg", "What does this audio say?", "Aisha Sherif")]
		[InlineData(InteractionContentType.Video, "video/mp4", "What is happening in this video? Provide a timestamped summary.", "squirrel")]
		[InlineData(InteractionContentType.Document, "application/pdf", "What is this document about?", "DeepMind")]
		public async Task Multimodal_Understanding_using_FilesAPI(InteractionContentType type, string mimeType, string prompt, string expected)
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
					new() { Type = InteractionContentType.Text, Text = prompt },
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
				if (output.Type == InteractionContentType.Image)
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