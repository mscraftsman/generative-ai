using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

var apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
var model = "gemini-1.5-flash";
var prompt = "Explain to me how AI works.";

OpenAIClientOptions options = new() { Endpoint = new Uri("https://generativelanguage.googleapis.com/v1beta/") };
ChatClient client = new(model, new ApiKeyCredential(apiKey), options);
ChatCompletion completion = client.CompleteChat(prompt);
Console.WriteLine($"[ASSISTANT]: {completion.Content[0].Text}");

var response = client.CompleteChat(prompt);
Console.WriteLine($"[Raw]: {response.GetRawResponse()}");