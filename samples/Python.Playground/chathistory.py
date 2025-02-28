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

chat = client.chats.create(
    model="gemini-2.0-flash",
    config=types.GenerateContentConfig(
        system_instruction=system_instruction,
        tools=None,
    ),
    history=None,   # here you can inject a previous history of the chat as list[Content]
)

response = chat.send_message("Tell me about quantum computing.")
print(response.text)
response = chat.send_message("Now, like I'm 5 years old.")
print(response.text)
for message in chat._curated_history:
    print(f'role - ', message.role, end=": ")
    print(message.parts[0].text)
