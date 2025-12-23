using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Types;
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;

public class Program
{
    public static void Main(string[] args)
    {
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter()
            .Build();

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter()
            .Build();

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
            Console.WriteLine(
                "No API key found. See file 'appsettings.json' for instructions or set the GOOGLE_API_KEY environment variable.");
            return;
        }

// Create a new instance of the GenerativeModel class.
        var genai = new GoogleAI(apiKey);
        var model = genai.GenerativeModel(Model.Gemini25Flash);
// model.UseJsonMode = true;

// Create a loop to keep the program running until the user exits entering the Escape key.
        var hint = " (Press Escape to exit)";
        while (true)
        {
            // Prompt the user for a prompt
            Console.Write($"Enter a prompt{hint}: ");
            string request = Console.ReadLine() ?? string.Empty;

            // check input for Escape key and exit if found
            if (request == "\x1b")
            {
                break;
            }

            hint = string.Empty;

            // Send the prompt to Gemini (using Google AI with API key)
            var response = model.GenerateContent(request).Result;

            // Display the response
            Console.WriteLine($"Response: {response.Text}");
            Console.WriteLine();
        }
    }
}