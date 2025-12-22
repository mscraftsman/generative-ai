using Microsoft.Extensions.AI;
using Mscc.GenerativeAI.Microsoft;
using Mscc.GenerativeAI.Types;

var settings = new
{
	GoogleApiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY"),
	GoogleApiModel = Model.Gemini25Flash
};

try
{
	var chatClient = new GeminiChatClient(apiKey: settings.GoogleApiKey, model: settings.GoogleApiModel);

	var messages = new List<Microsoft.Extensions.AI.ChatMessage>
	{
		new(ChatRole.User, "Hi")
	};

	var additionalProperties = new AdditionalPropertiesDictionary { { "RetryStatusCodes", (int[]) [500, 503, 504] } };
	var options = new ChatOptions
	{
		MaxOutputTokens = 10,
		AdditionalProperties = additionalProperties
	};

	var response = await chatClient.GetResponseAsync(messages, options);
            
	var text = response.Text;

}
catch (GeminiApiTimeoutException ex)
{
	var error = $"Google Gemini API error: {ex.Message}";            
}
catch (TimeoutException ex)
{
	var error = $"Google Gemini API error: {ex.Message}";            
}
catch (GeminiApiException ex)
{
	var error = $"Google Gemini API error: {ex.Message}";
}
catch (Exception ex)
{
	var error = $"Google Gemini API error: {ex.Message}";
}
