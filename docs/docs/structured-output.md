# Structured output

You can configure Gemini for structured output instead of unstructured text, allowing precise extraction and standardization of information for further processing. For example, you can use structured output to extract information from resumes, standardize them to build a structured database.

Gemini can generate either JSON or enum values as structured output.

## Generating JSON

To constrain the model to generate JSON, configure a responseSchema. The model will then respond to any prompt with JSON-formatted output.

```csharp
using Mscc.GenerativeAI;

class Recipe {
    public string RecipeName { get; set; }
    public List<string> Ingredients { get; set; }
}

// generate structure JSON output
var prompt = "List a few popular cookie recipes, and include the amounts of ingredients.";
var googleAi = new GoogleAI();
var model = googleAi.GenerativeModel(model: Model.Gemini25Flash);
var generationConfig = new GenerationConfig()
{
    ResponseMimeType = "application/json",
    ResponseSchema = Schema.FromType<List<Recipe>>()
};

var response = await model.GenerateContent(prompt, 
    generationConfig: generationConfig);
Console.WriteLine(response.Text);
```

The output might look like this:

```json

```

## Generating enum values

In some cases you might want the model to choose a single option from a list of options. To implement this behavior, you can pass an enum in your schema. You can use an enum option anywhere you could use a string in the responseSchema, because an enum is an array of strings. Like a JSON schema, an enum lets you constrain model output to meet the requirements of your application.

For example, assume that you're developing an application to classify musical instruments into one of five categories: "Percussion", "String", "Woodwind", "Brass", or ""Keyboard"". You could create an enum to help with this task.

In the following example, you pass an enum as the responseSchema, constraining the model to choose the most appropriate option.

```csharp
using Mscc.GenerativeAI;

// Define the Instrument enum
public enum Instrument
{
    [EnumMember(Value = "Percussion")] Percussion,
    [EnumMember(Value = "String")] String,
    [EnumMember(Value = "Woodwind")] Woodwind,
    [EnumMember(Value = "Brass")] Brass,
    [EnumMember(Value = "Keyboard")] Keyboard
}

var googleAi = new GoogleAI();
var model = googleAi.GenerativeModel(model: _model);

var generationConfig = new GenerationConfig
{
    ResponseMimeType = "text/x.enum", // Important for enum handling
    ResponseSchema = typeof(Instrument) // Provide the enum type
};

var response = await model.GenerateContent("What type of instrument is an oboe?", 
    generationConfig: generationConfig);

Console.WriteLine($"Response: {response.Text}");

// Parse the enum (more robust error handling)
if (Enum.TryParse(response.Text, out Instrument instrument))
{
    Console.WriteLine($"Parsed Instrument: {instrument}");
}
else
{
    Console.WriteLine($"Could not parse '{response.Text}' as a valid Instrument enum.");
}
```

## Best practices

Keep the following considerations and best practices in mind when you're using a response schema:

- The size of your response schema counts towards the input token limit.
- By default, fields are optional, meaning the model can populate the fields or skip them. You can set fields as required to force the model to provide a value. If there's insufficient context in the associated input prompt, the model generates responses mainly based on the data it was trained on.

- A complex schema can result in an InvalidArgument: 400 error. Complexity might come from long property names, long array length limits, enums with many values, objects with lots of optional properties, or a combination of these factors.

  If you get this error with a valid schema, make one or more of the following changes to resolve the error:
  - Shorten property names or enum names.
  - Flatten nested arrays.
  - Reduce the number of properties with constraints, such as numbers with minimum and maximum limits.
  - Reduce the number of properties with complex constraints, such as properties with complex formats like date-time.
  - Reduce the number of optional properties.
  - Reduce the number of valid values for enums.

- If you aren't seeing the results you expect, add more context to your input prompts or revise your response schema. For example, review the model's response without structured output to see how the model responds. You can then update your response schema so that it better fits the model's output. For additional troubleshooting tips on structured output, see the troubleshooting guide.

## What's next

Now that you've learned how to generate structured output, you might want to try using Gemini API tools:

- [Function calling](function-calling.md)
- [Code execution](code-execution.md)
- [Grounding with Google Search](google-search.md)
