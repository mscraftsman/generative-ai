using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Microsoft;

namespace Console.Gemini.FunctionCalling;

class Program
{
    static async Task Main(string[] args)
    {
        var apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            System.Console.WriteLine("Please set the GOOGLE_API_KEY environment variable.");
            return;
        }

        string model = "gemini-1.5-flash-preview";
        var gemini = new GeminiChatClient(apiKey, model);

        IChatClient chatClient = new ChatClientBuilder(gemini)
                .UseFunctionInvocation()
                .Build();

        var getUserInformationTool = AIFunctionFactory.Create(GetUserInformation, name: "get_user_information");

        var options = new ChatOptions
        {
            Tools = [getUserInformationTool]
        };

        var chatHistory = new List<Microsoft.Extensions.AI.ChatMessage>();

        while (true)
        {
            System.Console.Write("User > ");
            string? prompt = System.Console.ReadLine();

            if (String.IsNullOrEmpty(prompt))
                return;

            var userChatMessage = new Microsoft.Extensions.AI.ChatMessage(ChatRole.User, prompt);
            chatHistory.Add(userChatMessage);

            var response = await chatClient.GetResponseAsync(chatHistory, options);
            var responseMessage = response.Messages.First();
            chatHistory.Add(responseMessage);

            if (responseMessage.Text is string content)
            {
                System.Console.WriteLine($"Assistant > {content}");
            }

            if (response.FinishReason == ChatFinishReason.ToolCalls)
            {
                System.Console.WriteLine("Assistant > Tool call requested.");
                // In a real application, you would execute the tool and send the result back to the model.
            }
        }
    }

    [Description("Get basic information of the current user")]
    static string GetUserInformation() => @"{ ""Name"": ""John Doe"", ""Age"": 42 }";
}