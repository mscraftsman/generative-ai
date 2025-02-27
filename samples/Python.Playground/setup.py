from setuptools import setup, find_packages


setup(
    name="playground",
    version="0.0.1",
    packages=find_packages(),
    install_requires=[
        "google-genai",  # for Gemini SDK
        "openai",        # for OpenAI client
        "httpx",         # for HTTP requests
        "pydantic",      # for structured output
        "python-dotenv", # for loading environment variables
        "pillow"         # for image processing
    ],
    author="Jochen Kirstätter",
    description="A playground in Python to work with Google's Gemini",
    python_requires=">=3.8",
    # entry_points={}
)