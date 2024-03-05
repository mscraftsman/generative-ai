using Mscc.GenerativeAI.Web;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.Configure<GenerativeAIOptions>(builder.Configuration.GetSection("Gemini"));
//builder.Services.AddGenerativeAI();
builder.Services.AddGenerativeAI(builder.Configuration.GetSection("Gemini"));
//builder.Services.AddGenerativeAI("Gemini");
//builder.Services.AddGenerativeAI(new GenerativeAIOptions
//{
//    ProjectId = Environment.GetEnvironmentVariable("GOOGLE_PROJECT_ID"),
//    Region = Environment.GetEnvironmentVariable("GOOGLE_REGION"),
//    Model = Environment.GetEnvironmentVariable("GOOGLE_MODEL"),
//    Credentials = new() { ApiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY") }
//});
//builder.Services.AddGenerativeAI(options =>
//{
//    options.ProjectId = string.Empty;
//});

var app = builder.Build();

app.MapGet("/", async (IGenerativeModelService service) =>
{
    var result = await service.GenerateContent("Write about the history of Mauritius.");
    return result.Text;
});
app.MapGet("/models", async (IGenerativeModelService service) => 
{
    var result = await service.ListModels();
    return result;
});
app.MapGet("/models/{model}", async (IGenerativeModelService service, string model) =>
{
    var result = await service.GetModel(model);
    return result;
});
app.MapGet("/text/{prompt}", async (IGenerativeModelService service, string prompt) =>
{
    var result = await service.GenerateContent(prompt);
    return result.Text;
});

app.Run();
