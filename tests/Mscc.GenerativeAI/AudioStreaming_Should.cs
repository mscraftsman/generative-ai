using Mscc.GenerativeAI.Types;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace Test.Mscc.GenerativeAI
{
    public class AudioStreaming_Should
    {
        [Fact]
        public void Part_Audio_Property_Should_Return_Bytes_When_MimeType_Is_Audio()
        {
            // Arrange
            var audioData = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            var base64Data = Convert.ToBase64String(audioData);
            var part = new Part
            {
                InlineData = new InlineData
                {
                    MimeType = "audio/wav",
                    Data = base64Data
                }
            };

            // Act
            var result = part.Audio;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBe(audioData);
        }

        [Fact]
        public void Part_Audio_Property_Should_Return_Null_When_MimeType_Is_Not_Audio()
        {
            // Arrange
            var imageData = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            var base64Data = Convert.ToBase64String(imageData);
            var part = new Part
            {
                InlineData = new InlineData
                {
                    MimeType = "image/png",
                    Data = base64Data
                }
            };

            // Act
            var result = part.Audio;

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void GenerateContentResponse_Audio_Property_Should_Return_Bytes_From_First_Candidate()
        {
            // Arrange
            var audioData = new byte[] { 0x05, 0x06, 0x07, 0x08 };
            var base64Data = Convert.ToBase64String(audioData);
            var response = new GenerateContentResponse
            {
                Candidates = new List<Candidate>
                {
                    new Candidate
                    {
                        Content = new ContentResponse
                        {
                            Parts = new List<Part>
                            {
                                new Part { Text = "Here is some audio." },
                                new Part
                                {
                                    InlineData = new InlineData
                                    {
                                        MimeType = "audio/mp3",
                                        Data = base64Data
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var result = response.Audio;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBe(audioData);
        }

        [Fact]
        public void GenerateContentResponse_Audio_Property_Should_Return_Null_When_No_Audio_Part()
        {
            // Arrange
            var response = new GenerateContentResponse
            {
                Candidates = new List<Candidate>
                {
                    new Candidate
                    {
                        Content = new ContentResponse
                        {
                            Parts = new List<Part>
                            {
                                new Part { Text = "No audio here." }
                            }
                        }
                    }
                }
            };

            // Act
            var result = response.Audio;

            // Assert
            result.ShouldBeNull();
        }
    }
}
