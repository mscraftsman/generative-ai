using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;

// Get the API key from the configuration
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile("appsettings.user.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();
var apiKey = configuration["Gemini:Credentials:ApiKey"];
if (string.IsNullOrEmpty(apiKey) || apiKey.Equals("YOUR_API_KEY"))
{
    apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
}

// Check if the API key is valid. If not, exit the program.
if (string.IsNullOrEmpty(apiKey) || apiKey.Equals("YOUR_API_KEY"))
{
    Console.WriteLine("No API key found. See file 'appsettings.json' for instructions or set the GOOGLE_API_KEY environment variable.");
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
var content = await model.GenerateContent(request);
Console.WriteLine($"Response: {content.Text}");
