using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI.Microsoft;
using Mscc.GenerativeAI.Types;
using Neovolve.Logging.Xunit;
using Shouldly;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using mea = Microsoft.Extensions.AI;

namespace Test.Mscc.GenerativeAI.Microsoft
{
	[Collection(nameof(ConfigurationFixture))]
	public class GeminiChatClient_Should : LoggingTestsBase
	{
		private readonly ITestOutputHelper _output;
		private readonly ConfigurationFixture _fixture;

		public GeminiChatClient_Should(ITestOutputHelper output, ConfigurationFixture fixture)
			: base(output, LogLevel.Trace)
		{
			_output = output;
			_fixture = fixture;
		}

		[Fact]
		public async Task GetResponseAsync()
		{
			// Arrange
			var model = "gemini-2.5-flash";
			var prompt = "What is AI?";
			IChatClient chatClient = new GeminiChatClient(apiKey: _fixture.ApiKey, model: model, logger: Logger);

			// Act
			var response = await chatClient.GetResponseAsync(prompt);

			// Assert
			_output.WriteLine(response.Text);
		}

		[Fact]
		public async Task GetResponseAsync_using_ChatHistory()
		{
			// Arrange
			var model = "gemini-2.5-flash";
			var prompt = "What is AI?";
			List<mea.ChatMessage> chatHistory = new List<mea.ChatMessage> { new(ChatRole.User, prompt) };
			IChatClient chatClient = new GeminiChatClient(apiKey: _fixture.ApiKey, model: model, logger: Logger);

			// Act
			var response = await chatClient.GetResponseAsync(chatHistory);

			// Assert
			_output.WriteLine(response.Text);
		}

		[Fact]
		public async Task GetResponseAsync_with_Web_Search()
		{
			// Arrange
			var model = "gemini-2.5-flash";
			var prompt = "Who won the 2025 F1 Championship?";
			IChatClient chatClient = new GeminiChatClient(apiKey: _fixture.ApiKey, model: model, logger: Logger);
			var chatOptions = new ChatOptions { Tools = [new HostedWebSearchTool()] };

			// Act
			var response = await chatClient.GetResponseAsync(prompt, chatOptions);

			// Assert
			_output.WriteLine(response.Text);
		}

		[Fact]
		public async Task GetResponseAsync_with_Google_Search()
		{
			// Arrange
			var model = "gemini-2.5-flash";
			var prompt = "Who won the 2025 F1 Championship?";
			IChatClient chatClient = new GeminiChatClient(apiKey: _fixture.ApiKey, model: model, logger: Logger);
			// var search = new GoogleSearch{ ExcludeDomains = ["google.com"], BlockingConfidence = PhishBlockThreshold.BlockHighAndAbove };
			var search = new GoogleSearch { };
			var chatOptions = new ChatOptions { Tools = [search.AsAITool()] };

			// Act
			var response = await chatClient.GetResponseAsync(prompt, chatOptions);

			// Assert
			_output.WriteLine(response.Text);
		}

		[Fact]
		public async Task GetResponseAsync_with_Enterprise_Web_Search()
		{
			// Arrange
			var model = "gemini-2.5-flash";
			var prompt = "Who won the 2025 F1 Championship?";
			IChatClient chatClient = new GeminiChatClient(projectId: _fixture.ProjectId,
				region: _fixture.Region,
				accessToken: _fixture.AccessToken,
				model: model,
				logger: Logger);
			var search = new EnterpriseWebSearch
			{
				ExcludeDomains = ["google.com"], BlockingConfidence = BlockingConfidence.BlockHighAndAbove
			};
			var chatOptions = new ChatOptions { Tools = [search.AsAITool()] };

			// Act
			var response = await chatClient.GetResponseAsync(prompt, chatOptions);

			// Assert
			_output.WriteLine(response.Text);
		}

		[Fact]
		public async Task GetResponseAsync_with_Vertex_AI_Search()
		{
			// Arrange
			var model = "gemini-2.5-flash";
			var prompt = "How can I use this SDK?";
			var dataStoreId = "123456";
			IChatClient chatClient = new GeminiChatClient(projectId: _fixture.ProjectId,
				region: _fixture.Region,
				accessToken: _fixture.AccessToken,
				model: model,
				logger: Logger);
			var search = new Retrieval
			{
				VertexAiSearch = new VertexAISearch
				{
					Datastore =
						$"projects/{_fixture.ProjectId}/locations/global/collections/default_collection/dataStores/{dataStoreId}"
				}
			};
			var chatOptions = new ChatOptions { Tools = [search.AsAITool()] };

			// Act
			var response = await chatClient.GetResponseAsync(prompt, chatOptions);

			// Assert
			_output.WriteLine(response.Text);
		}

		[Fact]
		public async Task GetResponseAsync_with_External_Search_API()
		{
			// Arrange
			var model = "gemini-2.5-flash";
			var prompt = "How can I use this SDK?";
			IChatClient chatClient = new GeminiChatClient(projectId: _fixture.ProjectId,
				region: _fixture.Region,
				accessToken: _fixture.AccessToken,
				model: model,
				logger: Logger);
			var search = new Retrieval
			{
				ExternalApi = new ExternalApi()
				{
					ApiSpec = ExternalApi.ApiSpecType.SimpleSearch,
					Endpoint = "https://google.com/",
					ApiAuth = new() { ApiKeyConfig = new() { ApiKeyString = "123456" } },
					// SimpleSearchParams = new SimpleSearchParams() { }
				}
			};
			var chatOptions = new ChatOptions { Tools = [search.AsAITool()] };

			// Act
			var response = await chatClient.GetResponseAsync(prompt, chatOptions);

			// Assert
			_output.WriteLine(response.Text);
		}

		[Fact]
		public async Task GetResponseAsync_with_Elasticsearch()
		{
			// Arrange
			var model = "gemini-2.5-flash";
			var prompt = "How can I use this SDK?";
			IChatClient chatClient = new GeminiChatClient(projectId: _fixture.ProjectId,
				region: _fixture.Region,
				accessToken: _fixture.AccessToken,
				model: model,
				logger: Logger);
			var search = new Retrieval
			{
				ExternalApi = new ExternalApi()
				{
					ApiSpec = ExternalApi.ApiSpecType.ElasticSearch,
					Endpoint = "",
					ApiAuth = new() { ApiKeyConfig = new() { ApiKeyString = "123456" } },
					ElasticSearchParams = new() { Index = "", SearchTemplate = "", NumHits = 10 }
				}
			};
			var chatOptions = new ChatOptions { Tools = [search.AsAITool()] };

			// Act
			var response = await chatClient.GetResponseAsync(prompt, chatOptions);

			// Assert
			_output.WriteLine(response.Text);
		}

		[Fact]
		public async Task GetResponseAsync_with_Google_Maps()
		{
			// Arrange
			var model = "gemini-2.5-flash";
			var prompt = "What are the best Italian restaurants within a 15-minute drive from here?";
			IChatClient chatClient = new GeminiChatClient(apiKey: _fixture.ApiKey, model: model, logger: Logger);
			var maps = new GoogleMaps { EnableWidget = true };
			var chatOptions = new ChatOptions
			{
				Tools = [maps.AsAITool()],
				AdditionalProperties = new AdditionalPropertiesDictionary
				{
					[nameof(ToolConfig)] = new ToolConfig()
					{
						RetrievalConfig = new RetrievalConfig()
						{
							LatLng = new LatLng() { Latitude = -20.283700f, Longitude = 57.371529f },
							LanguageCode = "de_DE"
						}
					}
				}
			};

			// Act
			var response = await chatClient.GetResponseAsync(prompt, chatOptions);

			// Assert
			_output.WriteLine(response.Text);
		}

		[Fact]
		public async Task GetResponseAsync_with_Url_Context()
		{
			// Arrange
			var model = "gemini-2.5-flash";
			var prompt = "What is the content of https://jochen.kirstaetter.name/?";
			IChatClient chatClient = new GeminiChatClient(apiKey: _fixture.ApiKey, model: model, logger: Logger);
			var context = new UrlContext();
			var chatOptions = new ChatOptions { Tools = [context.AsAITool()] };

			// Act
			var response = await chatClient.GetResponseAsync(prompt, chatOptions);

			// Assert
			_output.WriteLine(response.Text);
		}

		[Fact]
		public async Task GetResponseAsync_with_Code_Execution()
		{
			// Arrange
			var model = "gemini-2.5-flash";
			var prompt =
				"What is the sum of the first 42 fibonacci numbers? Generate and run code to do the calculation?";
			IChatClient chatClient = new GeminiChatClient(apiKey: _fixture.ApiKey, model: model, logger: Logger);
			var chatOptions = new ChatOptions { Tools = [new HostedCodeInterpreterTool()] };

			// Act
			var response = await chatClient.GetResponseAsync(prompt, chatOptions);

			// Assert
			_output.WriteLine(response.Text);
		}

		public async Task GetResponseAsync_with_File_Search()
		{
			// Arrange
			var model = "gemini-2.5-flash";
			var prompt =
				"What is the sum of the first 42 fibonacci numbers? Generate and run code to do the calculation?";
			IChatClient chatClient = new GeminiChatClient(apiKey: _fixture.ApiKey, model: model, logger: Logger);
			var chatOptions = new ChatOptions { Tools = [new HostedFileSearchTool()] };

			// Act
			var response = await chatClient.GetResponseAsync(prompt, chatOptions);

			// Assert
			_output.WriteLine(response.Text);
		}

		public async Task GetResponseAsync_with_Image_Generation()
		{
			// Arrange
			var model = "gemini-2.5-flash";
			var prompt =
				"What is the sum of the first 42 fibonacci numbers? Generate and run code to do the calculation?";
			IChatClient chatClient = new GeminiChatClient(apiKey: _fixture.ApiKey, model: model, logger: Logger);
#pragma warning disable MEAI001
			var chatOptions = new ChatOptions { Tools = [new HostedImageGenerationTool()] };
#pragma warning restore MEAI001

			// Act
			var response = await chatClient.GetResponseAsync(prompt, chatOptions);

			// Assert
			_output.WriteLine(response.Text);
		}

		public async Task GetResponseAsync_with_MCP_Server()
		{
			// Arrange
			var model = "gemini-2.5-flash";
			var prompt =
				"What is the sum of the first 42 fibonacci numbers? Generate and run code to do the calculation?";
			IChatClient chatClient = new GeminiChatClient(apiKey: _fixture.ApiKey, model: model, logger: Logger);
#pragma warning disable MEAI001
			var chatOptions = new ChatOptions
			{
				Tools = [new HostedMcpServerTool("github", "https://api.githubcopilot.com/mcp/")]
			};
#pragma warning restore MEAI001

			// Act
			var response = await chatClient.GetResponseAsync(prompt, chatOptions);

			// Assert
			_output.WriteLine(response.Text);
		}

		[Theory]
		[InlineData("What is the user's name and age?")]
		[InlineData("Who am I?")]
		public async Task Handle_AIFunction_Tool(string prompt)
		{
			// Arrange
			var model = Model.Gemini25Pro;
			var gemini = new GeminiChatClient(apiKey: _fixture.ApiKey, model, logger: Logger);
			IChatClient chatClient = new ChatClientBuilder(gemini)
				.UseFunctionInvocation()
				.Build();

			AIFunction getUserInformationTool =
				AIFunctionFactory.Create(GetUserInformation, name: "get_user_information");

			var options = new ChatOptions { Tools = [getUserInformationTool] };

			var chatHistory = new List<mea.ChatMessage> { new(ChatRole.User, prompt) };

			// Act
			var response = await chatClient.GetResponseAsync(chatHistory, options);

			// Assert
			response.ShouldNotBeNull();
			response.Messages.ShouldNotBeNull();
			response.Messages.Count.ShouldBe(3);
			response.Messages[0].Contents.ShouldNotBeNull();
			response.Messages[0].Contents.Count.ShouldBeGreaterThanOrEqualTo(1);
			var functionCallContent = response.Messages[0].Contents[0] as FunctionCallContent;
			functionCallContent.ShouldNotBeNull();
			functionCallContent?.Name.ShouldBe("get_user_information");
			_output.WriteLine(response.Text);
		}

		[Theory]
		[InlineData("What is the user's name and age?")]
		[InlineData("Who am I?")]
		public async Task Handle_AIFunction_Tool_Streaming(string prompt)
		{
			// Arrange
			var model = Model.Gemini25Pro;
			var gemini = new GeminiChatClient(apiKey: _fixture.ApiKey, model);
			IChatClient chatClient = new ChatClientBuilder(gemini)
				.UseFunctionInvocation()
				.Build();

			AIFunction getUserInformationTool =
				AIFunctionFactory.Create(GetUserInformation, name: "get_user_information");

			var options = new ChatOptions { Tools = [getUserInformationTool] };

			var chatHistory = new List<mea.ChatMessage> { new(ChatRole.User, prompt) };

			// Act
			IAsyncEnumerable<ChatResponseUpdate> responseStream =
				chatClient.GetStreamingResponseAsync(chatHistory, options);

			// Assert
			responseStream.ShouldNotBeNull();
			await foreach (ChatResponseUpdate responseUpdate in responseStream)
			{
				responseUpdate.ShouldNotBeNull();
				// responseUpdate.Messages.ShouldNotBeNull();
				// responseUpdate.Messages.Count.ShouldBe(3);
				// responseUpdate.Messages[0].Contents.ShouldNotBeNull();
				// responseUpdate.Messages[0].Contents.Count.ShouldBeGreaterThanOrEqualTo(1);
				// var functionCallContent = responseUpdate.Messages[0].Contents[0] as mea.FunctionCallContent;
				// functionCallContent.ShouldNotBeNull();
				// functionCallContent?.Name.ShouldBe("get_user_information");
				_output.WriteLine(responseUpdate.Text);
			}
		}

		[Fact]
		public async Task Dotnet_Genai_Sample()
		{
			// assuming credentials are set up in environment variables.
			IChatClient chatClient = new GeminiClient(apiKey: _fixture.ApiKey)
				.AsIChatClient("gemini-2.5-flash")
				.AsBuilder()
				.UseFunctionInvocation()
				.UseOpenTelemetry()
				.Build();

			ChatOptions options = new()
			{
				Tools =
				[
					AIFunctionFactory.Create(
						([Description("The name of the person whose age is to be retrieved")] string personName) =>
							personName switch
							{
								"Alice" => 30,
								"Bob" => 25,
								_ => 35
							}, "get_person_age", "Gets the age of the specified person")
				]
			};

			await foreach (var update in chatClient.GetStreamingResponseAsync(
				               "How much older is Alice than Bob?", options))
			{
				_output.WriteLine(update.Text);
			}
		}

		[Fact]
		public async Task Test_Issue_163()
		{
			// var model = "gemini-2.5-flash";
			var model = Model.Gemini3Pro; 
			// var model = "gemini-3-flash-preview";
			var nativeClient = new GeminiChatClient(apiKey: _fixture.ApiKey, model: model, logger: Logger);

			var chatClient = nativeClient
				.AsBuilder()
				.UseFunctionInvocation(loggerFactory: null, configure: (functionInvokingClient) =>
				{
					functionInvokingClient.MaximumIterationsPerRequest = 999;
					functionInvokingClient.IncludeDetailedErrors = true;
				})
				.Build();

			var userMessage = new mea.ChatMessage(ChatRole.User, "What is 2 + 2?");

			_output.WriteLine($"Q : {userMessage.Text}");

			var output = await chatClient.GetResponseAsync<int>(userMessage);

			_output.WriteLine($"A : {output.Result}");
		}

		[Description("Get basic information of the current user, eg. name, age, etc.")]
		private static string GetUserInformation() => @"{ ""Name"": ""John Doe"", ""Age"": 42 }";
	}
}