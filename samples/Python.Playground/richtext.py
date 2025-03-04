import os
from google import genai
from google.genai import types
import httpx
from dotenv import load_dotenv

load_dotenv()  # take environment variables from .env.
client = genai.Client(
    api_key=os.environ['GOOGLE_API_KEY']
)

doc_url = "https://github.com/mscraftsman/generative-ai/blob/main/tests/Mscc.GenerativeAI/payload/GeminiDocumentProcessing.rtf"  # Replace with the actual URL of your PDF

# Retrieve and encode the PDF byte
doc_data = httpx.get(doc_url).content

prompt = "Summarize this document"
response = client.models.generate_content(
  model="gemini-2.0-flash",
  contents=[
      types.Part.from_bytes(
        data=doc_data,
        mime_type='text/rtf',
      ),
      prompt])
print(response.text)

response = client.models.generate_content(
  model="gemini-1.5-flash",
  contents=[
      types.Part.from_bytes(
        data=doc_data,
        mime_type='application/rtf',
      ),
      prompt])
print(response.text)
