var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGenerativeAI(builder.Configuration.GetSection("Gemini"));

var app = builder.Build();

app.MapGet("/", async (IGenerativeModelService service) =>
{
    var result = await service.GenerateContent("Write about the history of Mauritius.");
    return result.Text;
});

app.Run();
