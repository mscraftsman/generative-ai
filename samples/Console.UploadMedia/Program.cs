using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Types;

// Enables reading api key from dotnet user-secrets
IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

var googleAI = new GoogleAI(apiKey: config["GEMINI-API-KEY"]);
var model = googleAI.GenerativeModel(model: Model.Gemini20Flash);

string prompt = "Summerize the contents of the provided PDF file as a bullet point list";

var request = new GenerateContentRequest(prompt);

// Uploading files this way has a limit of 20mb
await request.AddMedia("./ten_lessons.pdf");

var response = await model.GenerateContent(request);

Console.WriteLine(response.Text);