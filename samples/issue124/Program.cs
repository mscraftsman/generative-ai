using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI;

using ILoggerFactory factory = LoggerFactory.Create(builder => builder
	.SetMinimumLevel(LogLevel.Debug)
	.AddConsole());
ILogger logger = factory.CreateLogger("Program");

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

var genai = new GoogleAI(apiKey, logger: logger);
var model = genai.GenerativeModel(Model.Gemini25Flash);

var request = new GenerateContentRequest(prompt);
var options = new RequestOptions { Timeout = TimeSpan.FromSeconds(300) };

var target = Path.Combine(Directory.GetCurrentDirectory(), "data");
Directory.CreateDirectory(target);

foreach (var file in Directory.GetFiles(target)) {
    if (!string.IsNullOrEmpty(file)) {
        FileInfo fileInfo = new FileInfo(file);
        await request.AddMedia(fileInfo.FullName);
    }
}

var content = await model.GenerateContent(request, requestOptions: options);
// var content = await model.GenerateContent(request);
Console.WriteLine($"Response: {content.Text}");
