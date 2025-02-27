from setuptools import setup, find_packages
from setuptools.command.develop import develop
from setuptools.command.install import install
import os


class PostDevelopCommand(develop):
    """Post-installation for development mode."""

    def run(self):
        develop.run(self)
        create_env_file_if_not_exists()


class PostInstallCommand(install):
    """Post-installation for installation mode."""

    def run(self):
        install.run(self)
        create_env_file_if_not_exists()


def create_env_file_if_not_exists(filepath=".env"):
    """
    Checks if a .env file exists and creates it with a default GOOGLE_API_KEY entry if it doesn't.

    Args:
        filepath (str): The path to the .env file. Defaults to ".env" in the current directory.
    """
    if not os.path.exists(filepath):
        print(f"Creating .env file at {filepath}")
        with open(filepath, "w") as f:
            f.write("GOOGLE_API_KEY=\n")
        print(f".env file created successfully at {filepath}")
    else:
        print(f".env file already exists at {filepath}")


setup(
    name="playground",
    version="0.0.1",
    packages=find_packages(),
    install_requires=[
        "google-genai",  # for Gemini SDK
        "openai",  # for OpenAI client
        "httpx",  # for HTTP requests
        "pydantic",  # for structured output
        "python-dotenv",  # for loading environment variables
        "pillow"  # for image processing
    ],
    author="Jochen Kirstätter",
    description="A playground in Python to work with Google's Gemini",
    python_requires=">=3.8",
    cmdclass={
        'develop': PostDevelopCommand,
        'install': PostInstallCommand,
    },
)

create_env_file_if_not_exists()
