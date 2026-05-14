using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Mscc.GenerativeAI.Microsoft;

namespace Mscc.GenerativeAI.Samples.MicrosoftAiChatOptions;

class Program
{
    static async Task Main(string[] args)
    {
        var apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");

        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("Please set the GOOGLE_API_KEY environment variable.");
            return;
        }

        Console.WriteLine("Initializing GeminiChatClient...");

        // Initialize the GeminiChatClient using the Gemini API key and default model.
        IChatClient chatClient = new GeminiChatClient(apiKey: apiKey, model: "gemini-2.5-flash");

        Console.WriteLine("Preparing request with custom ChatOptions (Timeout and Retry logic)...");

        // Use standard Microsoft.Extensions.AI ChatOptions
        var options = new ChatOptions
        {
            MaxOutputTokens = 100,
            Temperature = 0.7f,
            
            // Mscc.GenerativeAI.Microsoft allows setting Gemini-specific RequestOptions
            // (e.g. timeouts and retries) via the AdditionalProperties dictionary.
            AdditionalProperties = new AdditionalPropertiesDictionary
            {
                // Set an explicit HTTP timeout for the request
                { "Timeout", TimeSpan.FromSeconds(30) },
                
                // Configure automated retry behavior (useful for HTTP 429 Rate Limit responses)
                { "RetryInitial", 100 },                 // Initial delay in milliseconds
                { "RetryMultiplies", 2 },                // Delay multiplier for exponential backoff
                { "RetryMaximum", 60000 },               // Maximum delay in milliseconds between retries
                { "RetryTimeout", 120 },                 // Overall timeout for all retries in seconds
                { "RetryStatusCodes", new[] { 429, 503 } } // Specific HTTP Status codes that trigger a retry
            }
        };

        var prompt = "What are the three laws of robotics?";
        Console.WriteLine($"\nPrompt: {prompt}");

        try
        {
            // Execute the request with our defined timeout/retry options
            var response = await chatClient.GetResponseAsync(prompt, options);

            Console.WriteLine("\nResponse received:\n");
            Console.WriteLine(response.Text);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError during request: {ex.Message}");
        }
    }
}
