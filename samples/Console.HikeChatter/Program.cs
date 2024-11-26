using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;

// Read the API key from the user secrets file.
var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string apiKey = config["GOOGLE_API_KEY"];

// == Providing context for the AI model ==========
var markdown = File.ReadAllText("hikes.md");
var systemPrompt = new Content(
"""
You are upbeat and friendly. You introduce yourself when first saying hello.
Provide a short answer only based on the user hiking records below:

""" + markdown);
Console.WriteLine($"\n\t-=-=- Hiking History -=-=--\n{markdown}");

// Create a new Google AI model client.
var genai = new GoogleAI(apiKey);
var generationConfig = new GenerationConfig { Temperature = 0f, TopP = 0.95f, MaxOutputTokens = 1000 };
var model = genai.GenerativeModel(model: Model.Gemini15Pro, generationConfig: generationConfig, systemInstruction: systemPrompt);

// == Starting the conversation ==========
string userGreeting = "Hi!";
Console.WriteLine($"\nUser >>> {userGreeting}");

var response = await model.GenerateContent(userGreeting);
Console.WriteLine($"\nGemini >>> {response.Text}");

// == Providing the user's request ==========
var hikeRequest = """
I would like to know the ration of hike I did in Canada compare to hikes done in other countries.
""";
Console.WriteLine($"\nUser >>> {hikeRequest}");

// == Getting the response from the AI model ==========
response = await model.GenerateContent(hikeRequest);
Console.WriteLine($"\nGemini >>> {response.Text}");
