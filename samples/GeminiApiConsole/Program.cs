using GeminiApiConsole;
using GeminiApiConsole.Apps;
using Mscc.GenerativeAI;
using Spectre.Console;

AnsiConsole.Clear();
Console.ResetColor();

AnsiConsole.Write(new Rule("Gemini API Console").LeftJustified());
AnsiConsole.WriteLine();

GenerativeModel? client = null;

var apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
if (apiKey is null)
{
    AnsiConsole.MarkupLine($"Enter the [{GeminiConsole.AccentTextColor}]Gemini API key[/]"); 
    apiKey = GeminiConsole.ReadInput();
}

try
{
    var genAi = new GoogleAI(apiKey);
    client = genAi.GenerativeModel("gemini-1.5-flash-latest");
    var models = await client.ListModels();
    if (!models.Any())
    {
        AnsiConsole.MarkupLineInterpolated($"[{GeminiConsole.WarningTextColor}]Your API key does not provide any models.[/]");
    }
    else
    {
        AnsiConsole.Write(new Rule("Available Gemini API models").LeftJustified());
        var sortedModels = models.OrderBy(m => m.DisplayName).ToList();
        sortedModels.ForEach(x =>
            AnsiConsole.MarkupLine($"  [{GeminiConsole.AccentTextColor}]{x.DisplayName}[/] ({x.Name})"));
    }
}
catch (Exception ex)
{
    AnsiConsole.MarkupLineInterpolated($"[{GeminiConsole.ErrorTextColor}]{Markup.Escape(ex.Message)}[/]");
    AnsiConsole.WriteLine();
}
GeminiConsole.ReadInput();

string demo;

do
{
    AnsiConsole.Clear();

    demo = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .PageSize(10)
            .Title("What demo do you want to run?")
            .AddChoices("Chat", "Image chat", "Tool chat", "Model manager", "Exit"));

    AnsiConsole.Clear();

    try
    {
        switch (demo)
        {
            case "Chat":
                await new ChatConsole(client!).Run();
                break;
            
            // case "Image chat":
            //     await new ImageChatConsole(client!).Run();
            //     break;
            //
            // case "Tool chat":
            //     await new ToolConsole(client!).Run();
            //     break;
            //
            // case "Model manager":
            //     await new ModelManagerConsole(client!).Run();
            //     break;
        }
    }
    catch (Exception ex)
    {
        AnsiConsole.MarkupLine($"An error occurred. Press [{GeminiConsole.AccentTextColor}]Return[/] to start over.");
        AnsiConsole.MarkupLineInterpolated($"[{GeminiConsole.ErrorTextColor}]{Markup.Escape(ex.Message)}[/]");
        Console.ReadLine();
    }
} while (demo != "Exit");
