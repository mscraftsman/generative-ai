import os
from google import genai
from google.genai import types
from dotenv import load_dotenv

load_dotenv()  # take environment variables from .env.
client = genai.Client(
    vertexai=True,
    project=os.environ['GOOGLE_PROJECT_ID'],
    location=os.environ.get('GOOGLE_CLOUD_LOCATION', 'us-central1')
)

google_maps_tool = types.Tool(
    google_maps=types.GoogleMaps(
        auth_config=types.AuthConfig(
            api_key_config=types.ApiKeyConfig(
                api_key_string=os.environ['GOOGLE_MAPS_API_KEY']
            )
        )
    )
)

system_instruction="""
You are a helpful assistant that provides information about locations.
Always use the Google Maps grounding source for your location-based information.
"""

prompt = "Nearby restaurant that has child-friendly atmosphere and 4.5+ reviews."

response = client.models.generate_content(
    model="gemini-2.5-flash",
    contents=prompt,
    config=types.GenerateContentConfig(
        system_instruction=system_instruction,
        tools=[google_maps_tool],
        tool_config=types.ToolConfig(
            retrieval_config=types.RetrievalConfig(
                lat_lng=types.LatLng(
                    latitude = -20.2646547, longitude = 57.4793535
                )
            )
        )
    )
)

print(response.text)
