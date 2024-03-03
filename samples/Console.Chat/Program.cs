using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;

var Configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile("appsettings.user.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var apiKey = Configuration["Gemini:Credentials:ApiKey"];
if (string.IsNullOrEmpty(apiKey))
{
    apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
}

var model = new GenerativeModel(apiKey);
Console.Write("Enter a prompt: ");
string request = Console.ReadLine();

var response = await model.GenerateContent(request);

Console.WriteLine($"Prompt: {request}");
Console.WriteLine($"Response: {response.Text}");
