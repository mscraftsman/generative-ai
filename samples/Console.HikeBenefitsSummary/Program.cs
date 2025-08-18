using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;

// Read the API key from the user secrets file.
var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string apiKey = config["GOOGLE_API_KEY"];

// Create a new Google AI model client.
var genai = new GoogleAI(apiKey);
var model = genai.GenerativeModel(model: Model.Gemini25Pro);

// Read the text to be summarized from a Markdown file.
var markdown = System.IO.File.ReadAllText("benefits.md");

// Instruct the model to summarize the text in 20 words or less.
var prompt = $"Please summarize the the following text in 20 words or less: {markdown}";
Console.WriteLine($"\nUser >>> {prompt}");

// Generate the summary and display it.
var response = await model.GenerateContent(prompt);
Console.WriteLine($"\nGemini >>> {response.Text}");