using Microsoft.Extensions.AI;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Microsoft;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using System.ComponentModel;
using mea = Microsoft.Extensions.AI;

GenerativeAIExtensions.ReadDotEnv();
var apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY")!;
var model = "gemini-2.5-pro";
var gemini = new GeminiChatClient(apiKey, model);

// // Before : Calling functions correctly. (OpenAI gpt-4.1)
// var chatClient1 = new OpenAIClient(apiKey)
//     .GetChatClient(model)
//     .AsIChatClient()
//     .AsBuilder()
//     .UseFunctionInvocation()
//     .Build();
//
// // After : Not calling functions. (Gemini 2.5 flash preview)
// var chatClient2 = new GeminiChatClient(apiKey, model)
//     .AsBuilder()
//     .UseFunctionInvocation()
//     .Build();
//
// var chatClient3 = new GeminiClient(apiKey)
//     .GetChatClient(model)
//     .AsIChatClient()
//     .AsBuilder()
//     .UseFunctionInvocation()
//     .Build();

IChatClient chatClient = new ChatClientBuilder(gemini)
    .UseFunctionInvocation()
    .Build();
		
AIFunction getUserInformationTool = AIFunctionFactory.Create(GetUserInformation, name: "get_user_information");

var options = new ChatOptions
{
    Tools = [getUserInformationTool]
};

List<mea.ChatMessage> chatHistory = [];

while (true)
{
    string? prompt = Console.ReadLine();

    if (String.IsNullOrEmpty(prompt))
        return;

    var userChatMessage = new mea.ChatMessage(ChatRole.User, prompt);
    chatHistory.Add(userChatMessage);
	
    IAsyncEnumerable<ChatResponseUpdate> responseStream = chatClient.GetStreamingResponseAsync(chatHistory, options);
	
    await foreach (ChatResponseUpdate responseUpdate in responseStream)
    {
        Console.Write(responseUpdate.Text);
        chatHistory.AddMessages(responseUpdate);
    }
    Console.WriteLine();
}

return;

[Description("Get basic information of the current user")]
static string GetUserInformation() => @"{ Name = ""John Doe"", Age = 42 }";