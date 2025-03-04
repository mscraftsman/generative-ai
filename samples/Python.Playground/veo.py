import os
from google import genai
from google.genai import types
import httpx
from dotenv import load_dotenv

load_dotenv()  # take environment variables from .env.
client = genai.Client(
    api_key=os.environ['GOOGLE_API_KEY']
)

# Ref: https://googleapis.github.io/python-genai/#veo

# Create operation
operation = client.models.generate_videos(
    model='veo-2.0-generate-001',
    prompt='A neon hologram of a cat driving at top speed',
    config=types.GenerateVideosConfig(
        number_of_videos=1,
        fps=24,
        duration_seconds=5,
        enhance_prompt=True,
    ),
)

# Poll operation
while not operation.done:
    time.sleep(20)
    operation = client.operations.get(operation)

video = operation.result.generated_videos[0].video
video.show()