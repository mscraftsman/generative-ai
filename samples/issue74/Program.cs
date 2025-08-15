using System.Text.Json;
using Mscc.GenerativeAI;
using Microsoft.Extensions.Logging;

using ILoggerFactory factory = LoggerFactory.Create(builder => builder
    .SetMinimumLevel(LogLevel.Debug)
    .AddConsole());
ILogger logger = factory.CreateLogger("Program");

string? apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
if (string.IsNullOrEmpty(apiKey))
{
    throw new Exception("API key is required.");
}

var googleAi = new GoogleAI(apiKey: apiKey, logger: logger);
var model = googleAi.GenerativeModel(Model.Gemini20Flash);

List<Tool> tools =
[
    new Tool()
    {
        FunctionDeclarations =
        [
            new()
            {
                Name = "find_event",
                Description = "search for an event in the documentation",
                Parameters = new()
                {
                    Type = ParameterType.Object,
                    Properties = new
                    {
                        Name = new
                        {
                            Type = ParameterType.String,
                            Description = "The name of the event to search for"
                        },
                    },
                    Required = ["name"]
                }
            },
        ]
    }
];

var chatSession = new ChatSession(model, tools: tools);

var prompt = "Is there an event when a player moves?";

var response = await chatSession.SendMessage(prompt);

await ProcessResponse(response, chatSession);

async Task ProcessResponse(GenerateContentResponse response, ChatSession chatSession)
{
    if (response.Candidates == null || response.Candidates.Count == 0)
    {
        Console.WriteLine("No response from the model.");
        return;
    }

    var candidate = response.Candidates.First();
    var parts = candidate.Content?.Parts ?? [];
    bool functionCalled = false;

    foreach (var part in parts)
    {
        if ((part.FunctionCall == null || string.IsNullOrEmpty(part.FunctionCall.Name)) && !functionCalled)
        {
            Console.WriteLine("AI's response:");
            Console.WriteLine(response.Text);

            return;
        }

        functionCalled = true;
        if (part.FunctionCall?.Name == "find_event")
        {
            var functionArgs = part.FunctionCall.Args as JsonElement?;
            string? name = functionArgs?.GetProperty("name").GetString();
            if (name != null)
            {
                Console.WriteLine($"Searching for event: {name}");
                string result = SearchEvent(name);
                Console.WriteLine($"Result: {result}");

                var functionResponsePart = new Part
                {
                    FunctionResponse = new FunctionResponse
                    {
                        Name = "find_event",
                        Response = new { content = result }
                    }
                };

                // Correct the wrongly logged model response!
                var received = chatSession.Last;
                received.Parts = candidate.Content.Parts;
                var history = chatSession.History;
                history.RemoveAt(history.Count - 1);
                history.Add(received);

                var followUpResponse = await chatSession.SendMessage([functionResponsePart]);
                // var request = new GenerateContentRequest([functionResponsePart]);
                
                // var followUpResponse = await chatSession.SendMessage(request);
                
                Console.WriteLine("AI's response with function results:");
                Console.WriteLine(followUpResponse.Text);
            }
        }
    }
}


string SearchEvent(string name)
{
    return name.ToLower() switch
    {
        "player move" => "on move",
        _ => "not found"
    };
}