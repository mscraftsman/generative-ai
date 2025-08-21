
using Mscc.GenerativeAI;

var prompt = "List a few popular cookie recipes.";
var googleAi = new GoogleAI(Environment.GetEnvironmentVariable("GOOGLE_API_KEY"));
var model = googleAi.GenerativeModel(Model.Gemini20Flash);
var generationConfig = new GenerationConfig()
{
    ResponseMimeType = "application/json", 
    ResponseSchema = new List<Recipe>()
};

var response = await model.GenerateContent(prompt,
    generationConfig: generationConfig);

Console.WriteLine(response?.Text);

// Define the types
class Recipe
{
    public required string RecipeName { get; set; }
    public List<Ingredient> Ingredients { get; set; }
}

class Ingredient
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public string Unit { get; set; }
}
