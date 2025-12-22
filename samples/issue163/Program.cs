using Microsoft.Extensions.AI;
using Mscc.GenerativeAI.Microsoft;
using Mscc.GenerativeAI.Types;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

var apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");

// var model = "gemini-2.5-flash";
var model = Model.Gemini3Pro;
// var model = "gemini-3-flash-preview";
var nativeClient = new GeminiChatClient(apiKey, model: model, logger: null);

var chatClient = nativeClient
	.AsBuilder()
	.UseFunctionInvocation(loggerFactory: null, configure: (functionInvokingClient) =>
	{
		functionInvokingClient.MaximumIterationsPerRequest = 999;
		functionInvokingClient.IncludeDetailedErrors = true;
	})
	.Build();

var userMessage = new ChatMessage(ChatRole.User, "What is 2 + 2?");

Console.WriteLine($"Q : {userMessage.Text}");

var output = await chatClient.GetResponseAsync<int>(userMessage);

Console.WriteLine($"A : {output.Result}");




public static class Extensions
{
	public static ChatClientBuilder AsBuilder(this IChatClient client) => new ChatClientBuilder(client);
}
