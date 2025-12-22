using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Types;
using System.Text;

using ILoggerFactory factory = LoggerFactory.Create(builder => builder
    .SetMinimumLevel(LogLevel.Debug)
    .AddConsole());
ILogger logger = factory.CreateLogger("Program");

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

var googleAI = new GoogleAI(logger: logger);
SafetySetting s = new() { Category = HarmCategory.HarmCategoryHateSpeech, Threshold = HarmBlockThreshold.BlockNone };

var model = googleAI.GenerativeModel(model: Model.Gemini25Flash,
    safetySettings: new List<SafetySetting>
    {
        new SafetySetting
        {
            Category = HarmCategory.HarmCategoryHateSpeech, Threshold = HarmBlockThreshold.BlockNone
        },
        new SafetySetting
        {
            Category = HarmCategory.HarmCategoryHarassment, Threshold = HarmBlockThreshold.BlockNone
        },
        new SafetySetting
        {
            Category = HarmCategory.HarmCategoryDangerousContent, Threshold = HarmBlockThreshold.BlockNone
        },
        new SafetySetting
        {
            Category = HarmCategory.HarmCategorySexuallyExplicit, Threshold = HarmBlockThreshold.BlockNone
        }
    },
    generationConfig:
    new GenerationConfig { Temperature = 0, ThinkingConfig = new ThinkingConfig() { IncludeThoughts = false, ThinkingBudget = 0} }
);
var sess = model.StartChat();
while (true)
{
    Console.Write("> ");
    string input = Console.ReadLine();
    var response = sess.SendMessage(input).Result;
    Console.WriteLine(response.Text);
    Console.WriteLine();
}