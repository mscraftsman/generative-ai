using Mscc.GenerativeAI;
using Spectre.Console;

namespace GeminiApiConsole.Apps;

public class ChatConsole(GenerativeModel client) : GeminiConsole(client)
{
    public override async Task Run()
    {
        AnsiConsole.Write(new Rule("Chat").LeftJustified());
        AnsiConsole.WriteLine();

        client.Model = await SelectModel("Select a model you want to chat with:");

        if (!string.IsNullOrEmpty(client.Model))
        {
            var keepChatting = true;
            var systemPrompt = ReadInput($"Define a system prompt [{HintTextColor}](optional)[/]");

            do
            {
                AnsiConsole.MarkupLine("");
                AnsiConsole.MarkupLine($"You are talking to [{AccentTextColor}]{client.Model}[/] now.");
                WriteChatInstructionHint();

                var chat = client.StartChat();

                string message;

                do
                {
                    AnsiConsole.WriteLine();
                    message = ReadInput();

                    if (message.Equals(EXIT_COMMAND, StringComparison.OrdinalIgnoreCase))
                    {
                        keepChatting = false;
                        break;
                    }

                    if (message.Equals(START_NEW_COMMAND, StringComparison.OrdinalIgnoreCase))
                    {
                        keepChatting = true;
                        break;
                    }

                    await foreach (var answerToken in chat.SendMessageStream(message))
                        AnsiConsole.MarkupInterpolated($"[{AiTextColor}]{answerToken}[/]");

                    AnsiConsole.WriteLine();
                } while (!string.IsNullOrEmpty(message));
            } while (keepChatting);
        }
    }
}