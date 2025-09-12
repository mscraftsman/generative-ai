using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using System.ClientModel;

var apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
var model = "gemini-2.5-flash";
var prompt = "Explain to me how AI works.";

OpenAIClientOptions options = new()
{
    Endpoint = new Uri("https://generativelanguage.googleapis.com/v1beta/openai/")
};
ChatClient client = new(model, new ApiKeyCredential(apiKey!), options);
ChatCompletion completion = client.CompleteChat(prompt);
Console.WriteLine($"[ASSISTANT]: {completion.Content[0].Text}");

var response = client.CompleteChat(prompt);
Console.WriteLine($"[Raw]: {response.GetRawResponse().Content}");

OpenAIModelClient modelClient = new(new ApiKeyCredential(apiKey!), options);
OpenAIModelCollection models = modelClient.GetModels();
foreach (OpenAIModel item in models)
{
    Console.WriteLine($"{item.Id} ({item.CreatedAt}) by {item.OwnedBy}");
}