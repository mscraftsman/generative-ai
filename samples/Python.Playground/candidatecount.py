import os
import json
from google import genai
from google.genai import types
from dotenv import load_dotenv

load_dotenv()  # take environment variables from .env.
client = genai.Client(
    api_key=os.environ['GOOGLE_API_KEY']
)

# Create a JSON structure large enough to trigger the error (~100K chars)
items = []
for i in range(1600):
    items.append({
        "id": i,
        "name": f"Item {i}",
    })

test_json = json.dumps({"items": items}, indent=4)
print(f"JSON size: {len(test_json):,} characters")

# Test: JSON with candidate_count=2 (will fail)
prompt = f"Analyze this JSON data:\n\n{test_json}"

# The key setting that triggers the error is candidate_count > 1
response = client.models.generate_content(
    model="gemini-exp-1206",
    contents=prompt,
    config=types.GenerateContentConfig(
        candidate_count=2,
    )
)
print(response.text)
print(response)
