using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

// Get the API key from the configuration
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile("appsettings.user.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();
var apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
if (string.IsNullOrEmpty(apiKey))
{
    apiKey = configuration["Gemini:Credentials:ApiKey"];
}

// Check if the API key is valid. If not, exit the program.
if (string.IsNullOrEmpty(apiKey) || apiKey.Equals("YOUR_API_KEY"))
{
    Console.WriteLine("No API key found. Please set the GOOGLE_API_KEY environment variable.");
    return;
}

var prompt = $@"Extract all headings from the attached CSV files.";

var genai = new GoogleAI(apiKey);
var model = genai.GenerativeModel("gemini-1.5-flash");

var request = new GenerateContentRequest(prompt);
var target = Path.Combine(Directory.GetCurrentDirectory(), "data");
Directory.CreateDirectory(target);

foreach (var file in Directory.GetFiles(target)) {
    if (!string.IsNullOrEmpty(file)) {
        FileInfo fileInfo = new FileInfo(file);
        await request.AddMedia(fileInfo.FullName);
    }
}

try
{
    // Create a CancellationTokenSource with a timeout of 300 seconds (5 minutes)
    var cts = new CancellationTokenSource(TimeSpan.FromSeconds(300));
    var content = await model.GenerateContent(request, cts.Token);
    Console.WriteLine($"Response: {content.Text}");
}
catch (TaskCanceledException)
{
    Console.WriteLine("The request timed out. Please try again with smaller files or a more specific prompt.");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
