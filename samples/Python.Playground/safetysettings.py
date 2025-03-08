import os
from google import genai
from google.genai import types
from dotenv import load_dotenv

load_dotenv()  # take environment variables from .env.
# client = genai.Client(
#     api_key=os.environ['GOOGLE_API_KEY']
# )
# client = genai.Client(
#     vertexai=True,
#     project='gemini-yourproject-123456',
#     location='us-central1'
# )
client = genai.Client(
    vertexai=True,
    api_key=os.environ['VERTEX_API_KEY']
)

prompt = "Explain me how your safety settings work."
response = client.models.generate_content(
    model="gemini-2.0-flash-lite-001",
    contents=prompt,
    config=types.GenerateContentConfig(
        temperature=1.0,
        response_mime_type="application/json",
        safety_settings=[
            types.SafetySetting(
                category=types.HarmCategory.HARM_CATEGORY_HATE_SPEECH,
                threshold=types.HarmBlockThreshold.BLOCK_NONE
            ),
            types.SafetySetting(
                category=types.HarmCategory.HARM_CATEGORY_HARASSMENT,
                threshold=types.HarmBlockThreshold.BLOCK_NONE
            ),
            types.SafetySetting(
                category=types.HarmCategory.HARM_CATEGORY_SEXUALLY_EXPLICIT,
                threshold=types.HarmBlockThreshold.BLOCK_NONE
            ),
            types.SafetySetting(
                category=types.HarmCategory.HARM_CATEGORY_DANGEROUS_CONTENT,
                threshold=types.HarmBlockThreshold.BLOCK_NONE
            ),
        ]
    )
)

print(response.text)
