import os
from google import genai
from google.genai import types
from dotenv import load_dotenv

load_dotenv()  # take environment variables from .env.
client = genai.Client(
    api_key=os.environ['GOOGLE_API_KEY']
)

system_instruction="""
  You are an expert software developer and a helpful coding assistant.
  You are able to generate high-quality code in any programming language.
"""

def set_light_values(brightness: int, color_temp: str) -> dict[str, int | str]:
    """Set the brightness and color temperature of a room light. (mock API).

    Args:
        brightness: Light level from 0 to 100. Zero is off and 100 is full brightness
        color_temp: Color temperature of the light fixture, which can be `daylight`, `cool` or `warm`.

    Returns:
        A dictionary containing the set brightness and color temperature.
    """
    return {
        "brightness": brightness,
        "colorTemperature": color_temp
    }

chat = client.chats.create(
    model="gemini-2.0-flash",
    config=types.GenerateContentConfig(
        system_instruction=system_instruction,
        tools=[set_light_values],
    ),
    history=None,   # here you can inject a previous history of the chat as list[Content]
)

response = chat.send_message("Turn the lights down to a romantic level")
print(response.text)
for message in chat._curated_history:
    print(f'role - ', message.role, end=": ")
    print(message.parts[0].text)
