using Microsoft.Agents.AI;
using Mscc.GenerativeAI.Microsoft;

const string JokerInstructions = "You are good at telling jokes.";
const string JokerName = "JokerAgent";

string apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY") ?? throw new InvalidOperationException("Please set the GOOGLE_API_KEY environment variable.");
string model = Environment.GetEnvironmentVariable("GOOGLE_GENAI_MODEL") ?? "gemini-2.5-flash";

// Using a community driven Mscc.GenerativeAI.Microsoft package

ChatClientAgent agentCommunity = new(
	new GeminiChatClient(apiKey: apiKey, model: model),
	name: JokerName,
	instructions: JokerInstructions);

AgentRunResponse response = await agentCommunity.RunAsync("Tell me a joke about a pirate.");
Console.WriteLine($"Community client based agent response:\n{response}");