import os
import google.generativeai as genai

genai.configure(api_key=os.environ['GOOGLE_API_KEY'])

imagen = genai.ImageGenerationModel("imagen-3.0-generate-001")

result = imagen.generate_images(
    prompt="Fuzzy bunnies in my kitchen",
    number_of_images=4,
    safety_filter_level="block_only_high",
    person_generation="allow_adult",
    aspect_ratio="3:4",
    negative_prompt="Outside",
)

for image in result.images:
  print(image)

# The output should look similar to this:
# <vertexai.preview.vision_models.GeneratedImage object at 0x78f3396ef370>
# <vertexai.preview.vision_models.GeneratedImage object at 0x78f3396ef700>
# <vertexai.preview.vision_models.GeneratedImage object at 0x78f33953c2b0>
# <vertexai.preview.vision_models.GeneratedImage object at 0x78f33953c280>

for image in result.images:
  # Open and display the image using your local operating system.
  image._pil_image.show()
