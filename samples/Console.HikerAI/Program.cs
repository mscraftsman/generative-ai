using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;

// Read the API key from the user secrets file.
var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string apiKey = config["GOOGLE_API_KEY"];

// == Providing context for the AI model ==========
var systemPrompt = new Content { Parts = new() { new TextData() { Text =
"""
You are a hiking enthusiast who helps people discover fun hikes in their area. You are upbeat and friendly.
You introduce yourself when first saying hello. When helping people out, you always ask them
for this information to inform the hiking recommendation you provide:

1. Where they are located
2. What hiking intensity they are looking for

You will then provide three suggestions for nearby hikes that vary in length after you get that information.
You will also share an interesting fact about the local nature on the hikes when making a recommendation.
"""}}};

// Create a new Google AI model client.
var genai = new GoogleAI(apiKey);
var model = genai.GenerativeModel(model: Model.Gemini15Pro, systemInstruction: systemPrompt);

// == Starting the conversation ==========
string userGreeting = """
Hi!
Apparently you can help me find a hike that I will like?
""";
Console.WriteLine($"\nUser >>> {userGreeting}");

var response = await model.GenerateContent(userGreeting);
Console.WriteLine($"\nGemini >>> {response.Text}");

// == Providing the user's request ==========
var hikeRequest =
"""
I live in Mauritius area and would like an easy hike. I don't mind driving a bit to get there.
I don't want the hike to be over 15 kilometers round trip. I'd consider a point-to-point hike.
I want the hike to be as isolated as possible. I don't want to see many people.
I would like it to be as bug free as possible.
""";
Console.WriteLine($"\nUser >>> {hikeRequest}");

// == Getting the response from the AI model ==========
response = await model.GenerateContent(hikeRequest);
Console.WriteLine($"\nGemini >>> {response.Text}");
