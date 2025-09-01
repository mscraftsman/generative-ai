using Mscc.GenerativeAI;

public class MovieRecommendation
{
    public static async Task GenerateAsync(string apiKey, string inputText)
    {
        // Initialize the Google AI client with the API key
        var googleAI = new GoogleAI(apiKey: apiKey);

        // Specify the model to use
        string modelName = Model.Gemini20Flash;

        // Create the content parts
        var modelText = new Content("Okay, based on your love for The Matrix, Blade Runner, and Interstellar, you seem to enjoy thought-provoking sci-fi with complex themes, strong visuals, and philosophical underpinnings. Here are 3 movies you might enjoy:\n\n1.  **Arrival (2016):** Like Interstellar, Arrival deals with complex scientific concepts and humanity's place in the universe. It focuses on a linguist tasked with communicating with alien visitors, and it explores themes of time, perception, and communication. The movie is visually stunning and emotionally resonant, offering a deeply thoughtful experience.\n\n2.  **Dark City (1998):** If you enjoy the dystopian atmosphere and philosophical questions raised by The Matrix and Blade Runner, you'll likely appreciate Dark City. This neo-noir sci-fi film follows a man who awakens with amnesia in a city where the night is perpetual, and reality is manipulated by mysterious beings. It's visually striking and full of mystery.\n\n3.  **Gattaca (1997):** Gattaca, like Blade Runner, explores themes of genetic engineering and societal control in a near-future setting. It follows a man born without genetic advantages who strives to achieve his dream of space travel by assuming another's identity. It's a compelling story with strong performances and raises interesting questions about destiny and free will.");
        modelText.Role = "model";
        var contents = new List<Content>()
        {
            new Content("My favorite movies are The Matrix, Blade Runner, and Interstellar."),
            //modelText,
            //new Content(inputText)
        };

        // Configure safety settings
        var safetySettings = new List<SafetySetting>()
        {
            new SafetySetting
            {
                Category = HarmCategory.HarmCategoryHarassment,
                Threshold = HarmBlockThreshold.BlockMediumAndAbove
            },
            new SafetySetting
            {
                Category = HarmCategory.HarmCategorySexuallyExplicit,
                Threshold = HarmBlockThreshold.BlockLowAndAbove
            }
        };

        // Configure the generation settings
        var generateContentConfig = new GenerateContentConfig()
        {
            SafetySettings = safetySettings,
            ResponseMimeType = "text/plain",
            SystemInstruction = new Content("You are a movie recommendation expert. I will provide you with a list of my favorite movies, and you will suggest 3 movies I might enjoy, explaining briefly why.")
        };

        // Create a generative model
        var model = googleAI.GenerativeModel(model: modelName);

        // Create the GenerateContentRequest
        var request = new GenerateContentRequest()
        {
            Contents = contents,
            GenerationConfig = generateContentConfig
        };

        // Generate content in streaming
        try
        {
            await foreach (var chunk in model.GenerateContentStream(request))
            {
                Console.Write(chunk.Text);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public static async Task Main(string[] args)
    {
        // Replace with your actual API key, you can use environment variables or user secrets to store the API key
        string apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
        string inputText = "I really enjoyed Oppenheimer, what else would you recommend?";

        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("Please set the GEMINI_API_KEY environment variable.");
            return;
        }

        await GenerateAsync(apiKey, inputText);
    }
}