using Mscc.GenerativeAI.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GenerativeAIOptions>(builder.Configuration.GetSection("Gemini"));
builder.Services.AddGenerativeAI(new Action<GenerativeAIOptions>(options =>
{
    options = builder.Configuration.GetSection("Gemini");
});

var app = builder.Build();

app.MapGet("/text", async (IGenerativeModelService service) =>
{
    var result = await service.GenerateContent("");
    return result.Text;
});
app.MapGet("/text/{prompt}", async (IGenerativeModelService service, string prompt) =>
{
    var result = await Service.GenerateContent(prompt);
    return result.Text;
});

app.Run();
