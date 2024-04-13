var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGenerativeAI(builder.Configuration.GetSection("Gemini"));

var app = builder.Build();

app.MapGet("/", async (IGenerativeModelService service) =>
{
    var model = service.CreateInstance();
    var result = await model.GenerateContent("Write about the history of Mauritius.");
    return result.Text;
});

app.Run();
