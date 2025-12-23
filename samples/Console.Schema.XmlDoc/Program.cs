using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Types;

namespace Console.Schema.XmlDoc;

public class Program
{
    public static async Task Main(string[] args)
    {
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder
            .SetMinimumLevel(LogLevel.Debug)
            .AddConsole());
        ILogger logger = factory.CreateLogger("Program");

        // Get the API key from the environment variable
        var apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            System.Console.WriteLine("Please set the GOOGLE_API_KEY environment variable.");
            return;
        }

        // Create a new generative model
        var genai = new GoogleAI(apiKey, logger: logger);
        var model = genai.GenerativeModel(Model.GeminiPro);

        // Define the prompt
        var prompt = "Please analyze the following review and provide a summary, the sentiment, and the language of the review. Review: 'The new Gemini SDK for .NET is amazing! It's so easy to use and the documentation is great.'";

        // Define the generation configuration
        var generationConfig = new GenerationConfig()
        {
            ResponseMimeType = "application/json",
            ResponseSchema = Mscc.GenerativeAI.Types.Schema.FromType<ReviewAnalysisOutputModel>(GetXmlDocumentationPath())
        };

        // Create the request
        var request = new GenerateContentRequest(prompt, generationConfig: generationConfig);

        // Generate the content
        var result = await model.GenerateContent(request);

        // Print the result
        System.Console.WriteLine(result.Text);
    }

    /// <summary>
    /// Gets the path to the XML documentation file.
    /// </summary>
    /// <returns>The path to the XML documentation file.</returns>
    static string GetXmlDocumentationPath()
    {
        var assembly = typeof(ReviewAnalysisOutputModel).Assembly;
        var assemblyName = assembly.GetName().Name;
        var xmlDocumentationFile = $"{assemblyName}.xml";
        var xmlDocumentationPath = Path.Combine(AppContext.BaseDirectory, xmlDocumentationFile);
        return xmlDocumentationPath;
    }
}

/// <summary>
/// Represents the output of a review analysis.
/// </summary>
public class ReviewAnalysisOutputModel
{
    /// <summary>
    /// A summary of the review. Do not put someone's real name.
    /// </summary>
    public string? ReviewSummary { get; set; }

    /// <summary>
    /// The sentiment of the review (e.g., Positive, Negative, Neutral).
    /// </summary>
    public Sentiment? Sentiment { get; set; }

    /// <summary>
    /// The language of the review (e.g., en, es, fr).
    /// </summary>
    public string? Language { get; set; }
}

/// <summary>
/// Sentiment classification.
/// </summary>
public enum Sentiment 
{
	/// <summary>
	/// No harm observed.
	/// </summary>
    Neutral,
	/// <summary>
	/// Encouraging.
	/// </summary>
    Positive,
	/// <summary>
	/// Stay away!
	/// </summary>
    Negative
}
