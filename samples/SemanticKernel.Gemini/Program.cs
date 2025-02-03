using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Google;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Plugins.Web;
using Microsoft.SemanticKernel.Plugins.Web.Bing;
using Microsoft.SemanticKernel.Plugins.Web.Google;

// using Microsoft.SemanticKernel.Connectors.OpenAI;
#pragma warning disable SKEXP0010
#pragma warning disable SKEXP0050
#pragma warning disable SKEXP0070

// Get the API key from the configuration
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile("appsettings.user.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();
var apiKey = configuration["Gemini:Credentials:ApiKey"];  //

ServiceCollection c = new();
c.AddGoogleAIGeminiChatCompletion("gemini-1.5-flash", apiKey, GoogleAIVersion.V1_Beta);
c.AddKernel();
// c.AddLogging(b => b.AddConsole().SetMinimalLevel(LogLevel.Trace));
c.AddSingleton<IFunctionInvocationFilter, PermissionFilter>();
IServiceProvider services = c.BuildServiceProvider();

Kernel kernel = services.GetRequiredService<Kernel>();
// kernel.ImportPluginFromType<Demographics>();
// kernel.ImportPluginFromObject(new WebSearchEnginePlugin(
//     new GoogleConnector(configuration["Search:Credentials:ApiKey"],
//         configuration["Search:Credentials:SearchEngineId"])));

var executionSettings = new OpenAIPromptExecutionSettings() { ResponseFormat = typeof(Demographics) };

PromptExecutionSettings settings =
    new GeminiPromptExecutionSettings()
    {
        ToolCallBehavior = GeminiToolCallBehavior.AutoInvokeKernelFunctions,
        Temperature = 0.8,
        MaxTokens = 8192, 
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(), 
        ResponseSchema = typeof(Demographics)
    };
IChatCompletionService chatService = services.GetRequiredService<IChatCompletionService>();
ChatHistory history = new();

while (true)
{
    Console.Write("Q: ");
    history.AddUserMessage(Console.ReadLine());
    
    var assistant = await chatService.GetChatMessageContentAsync(history, settings, kernel);
    history.Add(assistant);
    Console.WriteLine(assistant);
}

class Demographics
{
    [KernelFunction]
    public int GetPersonAge(string name)
    {
        return name switch
        {
            "JoKi" => 48,
            "Mary Jane" => 43,
            _ => 42
        };
    }
}

class PermissionFilter : IFunctionInvocationFilter
{
    public Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
    {
        Console.WriteLine($"Ok to invoke {context.Function.Name} with {string.Join(", ", context.Arguments)}");
        if (Console.ReadLine() == "y")
        {
            return next(context);
        }
        throw new Exception("Error: user denied request");
    }
}