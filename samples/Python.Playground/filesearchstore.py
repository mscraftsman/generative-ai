import os
import time

from google import genai
from google.genai import types
from dotenv import load_dotenv

load_dotenv()  # take environment variables from .env.
client = genai.Client(api_key=os.environ['GOOGLE_API_KEY'])

store = client.file_search_stores.create()

upload_op = client.file_search_stores.upload_to_file_search_store(
    file_search_store_name=store.name,
    file='payload/a11.txt'
)

while not upload_op.done:
    time.sleep(5)
    upload_op = client.operations.get(upload_op)

response = client.models.generate_content(
    model="gemini-2.5-flash",
    contents="What can you tell me about the LUA?",
    config=types.GenerateContentConfig(
        tools=[types.Tool(
            file_search=types.FileSearch(
                file_search_store_names=[store.name]
            )
        )]
    )
)
print(response.text)
