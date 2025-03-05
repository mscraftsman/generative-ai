import os
import base64
from openai import OpenAI
from pydantic import BaseModel
from dotenv import load_dotenv
from PIL import Image
from io import BytesIO

load_dotenv()  # take environment variables from .env.
client = OpenAI(
    api_key=os.environ['GOOGLE_API_KEY'],
    base_url="https://generativelanguage.googleapis.com/v1beta/openai/"
)


response = client.images.generate(
    model="imagen-3.0-generate-002",
    prompt="a portrait of a sheepadoodle wearing a cape",
    response_format='b64_json',
    n=1,
)

for image_data in response.data:
  image = Image.open(BytesIO(base64.b64decode(image_data.b64_json)))
  image.show()

# Generate content
response = client.chat.completions.create(
    model="gemini-1.5-flash",
    n=1,
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {
            "role": "user",
            "content": "Explain to me how AI works"
        }
    ]
)

print(response.choices[0].message)

# Generate streaming content
response = client.chat.completions.create(
  model="gemini-1.5-flash",
  messages=[
    {"role": "system", "content": "You are a helpful assistant."},
    {"role": "user", "content": "Hello!"}
  ],
  stream=True
)

for chunk in response:
    print(chunk.choices[0].delta)

# function calling
tools = [
  {
    "type": "function",
    "function": {
      "name": "get_weather",
      "description": "Get the weather in a given location",
      "parameters": {
        "type": "object",
        "properties": {
          "location": {
            "type": "string",
            "description": "The city and state, e.g. Chicago, IL",
          },
          "unit": {"type": "string", "enum": ["celsius", "fahrenheit"]},
        },
        "required": ["location"],
      },
    }
  }
]

messages = [{"role": "user", "content": "What's the weather like in Chicago today?"}]
response = client.chat.completions.create(
  model="gemini-1.5-flash",
  messages=messages,
  tools=tools,
  tool_choice="auto"
)

print(response)

# image understanding
# Function to encode the image
# def encode_image(image_path):
#   with open(image_path, "rb") as image_file:
#     return base64.b64encode(image_file.read()).decode('utf-8')
#
# # Getting the base64 string
# base64_image = "" # encode_image("Path/to/agi/image.jpeg")
#
# response = client.chat.completions.create(
#   model="gemini-1.5-flash",
#   messages=[
#     {
#       "role": "user",
#       "content": [
#         {
#           "type": "text",
#           "text": "What is in this image?",
#         },
#         {
#           "type": "image_url",
#           "image_url": {
#             "url":  f"data:image/jpeg;base64,{base64_image}"
#           },
#         },
#       ],
#     }
#   ],
# )
#
# print(response.choices[0])

# structured output
class CalendarEvent(BaseModel):
    name: str
    date: str
    participants: list[str]

completion = client.beta.chat.completions.parse(
    model="gemini-1.5-flash",
    messages=[
        {"role": "system", "content": "Extract the event information."},
        {"role": "user", "content": "John and Susan are going to an AI conference on Friday."},
    ],
    response_format=CalendarEvent,
)

print(completion.choices[0].message.parsed)

# embeddings
response = client.embeddings.create(
    input="Your text string goes here",
    model="text-embedding-004"
)

print(response.data[0].embedding)