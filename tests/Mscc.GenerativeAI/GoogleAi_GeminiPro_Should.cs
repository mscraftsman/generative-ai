#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
#endif
using FluentAssertions;
using Mscc.GenerativeAI;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class GoogleAi_GeminiPro_Should
    {
        private readonly ITestOutputHelper output;
        private readonly ConfigurationFixture fixture;
        private readonly string model = Model.Gemini10Pro;

        public GoogleAi_GeminiPro_Should(ITestOutputHelper output, ConfigurationFixture fixture)
        {
            this.output = output;
            this.fixture = fixture;
        }

        [Fact]
        public void Initialize_EnvVars()
        {
            // Arrange
            Environment.SetEnvironmentVariable("GOOGLE_API_KEY", fixture.ApiKey);
            var expected = Environment.GetEnvironmentVariable("GOOGLE_AI_MODEL") ?? Model.Gemini10Pro;

            // Act
            var model = new GenerativeModel();

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be(expected);
        }

        [Fact]
        public void Initialize_Default_Model()
        {
            // Arrange
            var expected = Environment.GetEnvironmentVariable("GOOGLE_AI_MODEL") ?? Model.Gemini10Pro;

            // Act
            var model = new GenerativeModel(apiKey: fixture.ApiKey);

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be(expected);
        }

        [Fact]
        public void Initialize_Model()
        {
            // Arrange
            var expected = this.model;

            // Act
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be(expected);
        }

        [Fact]
        public async void List_Models()
        {
            // Arrange
            var model = new GenerativeModel(apiKey: fixture.ApiKey);

            // Act
            var sut = await model.ListModels();

            // Assert
            sut.Should().NotBeNull();
            sut.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            sut.ForEach(x =>
            {
                output.WriteLine($"Model: {x.DisplayName} ({x.Name})");
                x.SupportedGenerationMethods.ForEach(m => output.WriteLine($"  Method: {m}"));
            });
        }

        [Fact]
        public async void List_Models_Using_OAuth()
        {
            // Arrange
            var model = new GenerativeModel { AccessToken = fixture.AccessToken };

            // Act
            var sut = await model.ListModels();

            // Assert
            sut.Should().NotBeNull();
            sut.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            sut.ForEach(x =>
            {
                output.WriteLine($"Model: {x.DisplayName} ({x.Name})");
                x.SupportedGenerationMethods.ForEach(m => output.WriteLine($"  Method: {m}"));
            });
        }

        [Fact]
        public async void List_Tuned_Models()
        {
            // Arrange
            var model = new GenerativeModel { AccessToken = fixture.AccessToken };

            // Act
            var sut = await model.ListModels(true);
            // var sut = await model.ListTunedModels();

            // Assert
            sut.Should().NotBeNull();
            sut.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            sut.ForEach(x =>
            {
                output.WriteLine($"Model: {x.DisplayName} ({x.Name})");
                x.TuningTask.Snapshots.ForEach(m => output.WriteLine($"  Snapshot: {m}"));
            });
        }

        [Theory]
        [InlineData(Model.Gemini10Pro001)]
        [InlineData(Model.GeminiProVision)]
        [InlineData(Model.BisonText)]
        [InlineData(Model.BisonChat)]
        [InlineData("tunedModels/number-generator-model-psx3d3gljyko")]
        public async void Get_Model_Information(string modelName)
        {
            // Arrange
            var model = new GenerativeModel(apiKey: fixture.ApiKey);

            // Act
            var sut = await model.GetModel(model: modelName);

            // Assert
            sut.Should().NotBeNull();
            sut.Name.Should().Be($"models/{modelName}");
            output.WriteLine($"Model: {sut.DisplayName} ({sut.Name})");
            sut.SupportedGenerationMethods.ForEach(m => output.WriteLine($"  Method: {m}"));
        }

        [Theory]
        [InlineData("tunedModels/number-generator-model-psx3d3gljyko")]
        public async void Get_TunedModel_Information_Using_ApiKey(string modelName)
        {
            // Arrange
            var model = new GenerativeModel(apiKey: fixture.ApiKey);

            
            // Act & Assert
            await Assert.ThrowsAsync<NotSupportedException>(() => model.GetModel(model: modelName));
        }

        [Theory]
        [InlineData(Model.Gemini10Pro001)]
        [InlineData(Model.GeminiProVision)]
        [InlineData(Model.BisonText)]
        [InlineData(Model.BisonChat)]
        [InlineData("tunedModels/number-generator-model-psx3d3gljyko")]
        public async void Get_Model_Information_Using_OAuth(string modelName)
        {
            // Arrange
            var model = new GenerativeModel { AccessToken = fixture.AccessToken };
            var expected = modelName;
            if (!expected.Contains("/"))
                expected = $"models/{expected}";

            // Act
            var sut = await model.GetModel(model: modelName);

            // Assert
            sut.Should().NotBeNull();
            sut.Name.Should().Be(expected);
            output.WriteLine($"Model: {sut.DisplayName} ({sut.Name})");
            if (sut.State is null)
            {
                sut?.SupportedGenerationMethods?.ForEach(m => output.WriteLine($"  Method: {m}"));
            }
            else
            {
                output.WriteLine($"State: {sut.State}");
            }
        }

        [Fact]
        public async void Generate_Content()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async void Generate_Content_MultiplePrompt()
        {
            // Arrange
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);
            var parts = new List<IPart>
            {
                new TextData { Text = "What is x multiplied by 2?" },
                new TextData { Text = "x = 42" }
            };

            // Act
            var response = await model.GenerateContent(parts);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            output.WriteLine(response?.Text);
            response.Text.Should().Be("84");
        }

        [Fact]
        public async void Generate_Content_Request()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            request.Contents.Add(new Content
            {
                Role = Role.User,
                Parts = new List<IPart> { new TextData { Text = prompt } }
            });

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async void Generate_Content_RequestConstructor()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);
            var request = new GenerateContentRequest(prompt);
            request.Contents[0].Role = Role.User;

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async void Generate_Content_Stream()
        {
            // Arrange
            var prompt = "How are you doing today?";
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);

            // Act
            var responseStream = model.GenerateContentStream(prompt);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                response.Text.Should().NotBeEmpty();
                output.WriteLine(response?.Text);
                // response.UsageMetadata.Should().NotBeNull();
                // output.WriteLine($"PromptTokenCount: {response?.UsageMetadata?.PromptTokenCount}");
                // output.WriteLine($"CandidatesTokenCount: {response?.UsageMetadata?.CandidatesTokenCount}");
                // output.WriteLine($"TotalTokenCount: {response?.UsageMetadata?.TotalTokenCount}");
            }
        }

        [Fact]
        public async void Generate_Content_Stream_Request()
        {
            // Arrange
            var prompt = "How are you doing today?";
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            request.Contents.Add(new Content
            {
                Role = Role.User,
                Parts = new List<IPart> { new TextData { Text = prompt } }
            });

            // Act
            var responseStream = model.GenerateContentStream(request);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                response.Text.Should().NotBeEmpty();
                output.WriteLine(response?.Text);
                // response.UsageMetadata.Should().NotBeNull();
                // output.WriteLine($"PromptTokenCount: {response?.UsageMetadata?.PromptTokenCount}");
                // output.WriteLine($"CandidatesTokenCount: {response?.UsageMetadata?.CandidatesTokenCount}");
                // output.WriteLine($"TotalTokenCount: {response?.UsageMetadata?.TotalTokenCount}");
            }
        }

        [Theory]
        [InlineData("How are you doing today?", 6)]
        [InlineData("What kind of fish is this?", 7)]
        [InlineData("Write a story about a magic backpack.", 8)]
        [InlineData("Write an extended story about a magic backpack.", 9)]
        public async void Count_Tokens(string prompt, int expected)
        {
            // Arrange
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);

            // Act
            var response = await model.CountTokens(prompt);

            // Assert
            response.Should().NotBeNull();
            response.TotalTokens.Should().Be(expected);
            output.WriteLine($"Tokens: {response?.TotalTokens}");
        }

        [Theory]
        [InlineData("How are you doing today?", 7)]
        [InlineData("What kind of fish is this?", 8)]
        [InlineData("Write a story about a magic backpack.", 9)]
        [InlineData("Write an extended story about a magic backpack.", 10)]
        public async void Count_Tokens_Request(string prompt, int expected)
        {
            // Arrange
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            request.Contents.Add(new Content
            {
                Role = Role.User,
                Parts = new List<IPart> { new TextData { Text = prompt } }
            });

            // Act
            var response = await model.CountTokens(request);

            // Assert
            response.Should().NotBeNull();
            response.TotalTokens.Should().Be(expected);
            output.WriteLine($"Tokens: {response?.TotalTokens}");
        }

        [Fact]
        public async void Start_Chat()
        {
            // Arrange
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);
            var chat = model.StartChat();
            var prompt = "How can I learn more about C#?";

            // Act
            var response = await chat.SendMessage(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async void Start_Chat_With_History()
        {
            // Arrange
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);
            var history = new List<ContentResponse>
            {
                new ContentResponse { Role = Role.User, Parts = new List<Part> { new Part("Hello") } },
                new ContentResponse { Role = Role.Model, Parts = new List<Part> { new Part("Hello! How can I assist you today?") } }
            };
            var chat = model.StartChat(history);
            var prompt = "How does electricity work?";

            // Act
            var response = await chat.SendMessage(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(prompt);
            output.WriteLine(response?.Text);
            //output.WriteLine(response?.PromptFeedback);
        }

        [Fact]
        // Refs:
        // https://cloud.google.com/vertex-ai/generative-ai/docs/multimodal/send-chat-prompts-gemini
        public async void Start_Chat_Multiple_Prompts()
        {
            // Arrange
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);
            var chat = model.StartChat();

            // Act
            var prompt = "Hello, let's talk a bit about nature.";
            var response = await chat.SendMessage(prompt);
            output.WriteLine(prompt);
            output.WriteLine(response?.Text);
            prompt = "What are all the colors in a rainbow?";
            response = await chat.SendMessage(prompt);
            output.WriteLine(prompt);
            output.WriteLine(response?.Text);
            prompt = "Why does it appear when it rains?";
            response = await chat.SendMessage(prompt);
            output.WriteLine(prompt);
            output.WriteLine(response?.Text);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
        }

        [Fact]
        // Refs:
        // https://ai.google.dev/tutorials/python_quickstart#chat_conversations
        public async void Start_Chat_Conversations()
        {
            // Arrange
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);
            var chat = model.StartChat();

            // Act
            _ = await chat.SendMessage("Hello, fancy brainstorming about IT?");
            _ = await chat.SendMessage("In one sentence, explain how a computer works to a young child.");
            _ = await chat.SendMessage("Okay, how about a more detailed explanation to a high schooler?");
            _ = await chat.SendMessage("Lastly, give a thorough definition for a CS graduate.");

            // Assert
            chat.History.ForEach(c =>
            {
                output.WriteLine($"{new string('-', 20)}");
                output.WriteLine($"{c.Role}: {c.Parts[0].Text}");
            });
        }

        [Fact]
        public async void Start_Chat_Streaming()
        {
            // Arrange
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);
            var chat = model.StartChat();
            var prompt = "How can I learn more about C#?";

            // Act
            var responseStream = chat.SendMessageStream(prompt);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                response.Text.Should().NotBeEmpty();
                output.WriteLine(response?.Text);
                // response.UsageMetadata.Should().NotBeNull();
                // output.WriteLine($"PromptTokenCount: {response?.UsageMetadata?.PromptTokenCount}");
                // output.WriteLine($"CandidatesTokenCount: {response?.UsageMetadata?.CandidatesTokenCount}");
                // output.WriteLine($"TotalTokenCount: {response?.UsageMetadata?.TotalTokenCount}");
            }
        }

        [Fact]
        // Ref: https://ai.google.dev/docs/function_calling
        public async void Function_Calling()
        {
            // Arrange
            var prompt = "Which theaters in Mountain View show Barbie movie?";
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);
            List<Tool> tools =
            [
                new Tool()
                {
                    FunctionDeclarations =
                    [
                        new()
                        {
                            Name = "find_movies",
                            Description =
                                "find movie titles currently playing in theaters based on any description, genre, title words, etc.",
                            Parameters = new()
                            {
                                Type = ParameterType.Object,
                                Properties = new 
                                {
                                    Location = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "The city and state, e.g. San Francisco, CA or a zip code e.g. 95616"
                                    },
                                    Description = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "Any kind of description including category or genre, title words, attributes, etc."
                                    }
                                }, 
                                Required = ["description"]
                            }
                        },


                        new()
                        {
                            Name = "find_theaters",
                            Description =
                                "find theaters based on location and optionally movie title which are is currently playing in theaters",
                            Parameters = new() { 
                                Type = ParameterType.Object, 
                                Properties = new
                                {
                                    Location = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "The city and state, e.g. San Francisco, CA or a zip code e.g. 95616"
                                    },
                                    Movie = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "Any movie title"
                                    } 
                                }, 
                                Required = ["location"] 
                            }
                        },


                        new()
                        {
                            Name = "get_showtimes",
                            Description = "Find the start times for movies playing in a specific theater",
                            Parameters = new()
                            {
                                Type = ParameterType.Object,
                                Properties = new 
                                {
                                    Location = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "The city and state, e.g. San Francisco, CA or a zip code e.g. 95616"
                                    },
                                    Movie = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "Any movie title"
                                    },
                                    Theater = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "Name of the theater" 
                                    },
                                    Date = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "Date for requested showtime"
                                    }
                                },
                                Required = ["location", "movie", "theater", "date"]
                            }
                        }
                    ]
                }
            ];

            // Act
            var response = await model.GenerateContent(prompt, tools: tools);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Should().NotBeNull();
            output.WriteLine(response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Name);
            output.WriteLine(response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Args?.ToString());
        }

        [Fact]
        // Ref: https://ai.google.dev/docs/function_calling#function-calling-one-and-a-half-turn-curl-sample
        public async void Function_Calling_MultiTurn()
        {
            // Arrange
            var prompt = "Which theaters in Mountain View show Barbie movie?";
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);
            List<Tool> tools =
            [
                new Tool()
                {
                    FunctionDeclarations =
                    [
                        new()
                        {
                            Name = "find_movies",
                            Description =
                                "find movie titles currently playing in theaters based on any description, genre, title words, etc.",
                            Parameters = new()
                            {
                                Type = ParameterType.Object,
                                Properties = new 
                                {
                                    Location = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "The city and state, e.g. San Francisco, CA or a zip code e.g. 95616"
                                    },
                                    Description = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "Any kind of description including category or genre, title words, attributes, etc."
                                    }
                                }, 
                                Required = ["description"]
                            }
                        },


                        new()
                        {
                            Name = "find_theaters",
                            Description =
                                "find theaters based on location and optionally movie title which are is currently playing in theaters",
                            Parameters = new() { 
                                Type = ParameterType.Object, 
                                Properties = new
                                {
                                    Location = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "The city and state, e.g. San Francisco, CA or a zip code e.g. 95616"
                                    },
                                    Movie = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "Any movie title"
                                    } 
                                }, 
                                Required = ["location"] 
                            }
                        },


                        new()
                        {
                            Name = "get_showtimes",
                            Description = "Find the start times for movies playing in a specific theater",
                            Parameters = new()
                            {
                                Type = ParameterType.Object,
                                Properties = new 
                                {
                                    Location = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "The city and state, e.g. San Francisco, CA or a zip code e.g. 95616"
                                    },
                                    Movie = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "Any movie title"
                                    },
                                    Theater = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "Name of the theater" 
                                    },
                                    Date = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "Date for requested showtime"
                                    }
                                },
                                Required = ["location", "movie", "theater", "date"]
                            }
                        }
                    ]
                }
            ];
            var request = new GenerateContentRequest(prompt, tools: tools);
            request.Contents[0].Role = Role.User;
            request.Contents.Add(new Content()
            {
                Role = Role.Model,
                Parts = new()
                {
                    new FunctionCall() { Name = "find_theaters", Args = new { Location = "Mountain View, CA", Movie = "Barbie" } }
                }
            });
            request.Contents.Add(new Content()
            {
                Role = Role.Function,
                Parts = new()
                {
                    new FunctionResponse() { Name = "find_theaters", Response = new
                    {
                        Name = "find_theaters", Content = new
                        {
                            Movie = "Barbie",
                            Theaters = new dynamic[] { new
                                {
                                    Name = "AMC Mountain View 16",
                                    Address = "2000 W El Camino Real, Mountain View, CA 94040"
                                }, new
                                {
                                    Name = "Regal Edwards 14",
                                    Address = "245 Castro St, Mountain View, CA 94040"
                                }
                            }
                        }
                    }}
                }
            });
            
            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
        }

        [Fact]
        // Ref: https://ai.google.dev/docs/function_calling#multi-turn-example-2
        public async void Function_Calling_MultiTurn_Multiple()
        {
            // Arrange
            var prompt = "Which theaters in Mountain View show Barbie movie?";
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);
            List<Tool> tools =
            [
                new Tool()
                {
                    FunctionDeclarations =
                    [
                        new()
                        {
                            Name = "find_movies",
                            Description =
                                "find movie titles currently playing in theaters based on any description, genre, title words, etc.",
                            Parameters = new()
                            {
                                Type = ParameterType.Object,
                                Properties = new 
                                {
                                    Location = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "The city and state, e.g. San Francisco, CA or a zip code e.g. 95616"
                                    },
                                    Description = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "Any kind of description including category or genre, title words, attributes, etc."
                                    }
                                }, 
                                Required = ["description"]
                            }
                        },


                        new()
                        {
                            Name = "find_theaters",
                            Description =
                                "find theaters based on location and optionally movie title which are is currently playing in theaters",
                            Parameters = new() { 
                                Type = ParameterType.Object, 
                                Properties = new
                                {
                                    Location = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "The city and state, e.g. San Francisco, CA or a zip code e.g. 95616"
                                    },
                                    Movie = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "Any movie title"
                                    } 
                                }, 
                                Required = ["location"] 
                            }
                        },


                        new()
                        {
                            Name = "get_showtimes",
                            Description = "Find the start times for movies playing in a specific theater",
                            Parameters = new()
                            {
                                Type = ParameterType.Object,
                                Properties = new 
                                {
                                    Location = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "The city and state, e.g. San Francisco, CA or a zip code e.g. 95616"
                                    },
                                    Movie = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "Any movie title"
                                    },
                                    Theater = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "Name of the theater" 
                                    },
                                    Date = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "Date for requested showtime"
                                    }
                                },
                                Required = ["location", "movie", "theater", "date"]
                            }
                        }
                    ]
                }
            ];
            var request = new GenerateContentRequest(prompt, tools: tools);
            request.Contents[0].Role = Role.User;
            request.Contents.Add(new Content()
            {
                Role = Role.Model,
                Parts = new()
                {
                    new FunctionCall() { Name = "find_theaters", Args = new { Location = "Mountain View, CA", Movie = "Barbie" } }
                }
            });
            request.Contents.Add(new Content()
            {
                Role = Role.Function,
                Parts = new()
                {
                    new FunctionResponse() { Name = "find_theaters", Response = new
                    {
                        Name = "find_theaters", Content = new
                        {
                            Movie = "Barbie",
                            Theaters = new dynamic[] { new
                                {
                                    Name = "AMC Mountain View 16",
                                    Address = "2000 W El Camino Real, Mountain View, CA 94040"
                                }, new
                                {
                                    Name = "Regal Edwards 14",
                                    Address = "245 Castro St, Mountain View, CA 94040"
                                }
                            }
                        }
                    }}
                }
            });
            request.Contents.Add(new Content()
            {
                Role = Role.Model,
                Parts = new()
                {
                    new TextData(){ Text = "OK. I found two theaters in Mountain View showing Barbie: AMC Mountain View 16 and Regal Edwards 14." }
                }
            });
            request.Contents.Add(new Content()
            {
                Role = Role.User,
                Parts = new()
                {
                    new TextData(){ Text = "Can we recommend some comedy movies on show in Mountain View?" }
                }
            });
            
            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Should().NotBeNull();
            output.WriteLine(response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Name);
            output.WriteLine(response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Args?.ToString());
        }

        [Fact(Skip = "Work in progress")]
        public async void Function_Calling_Chat()
        {
            // Arrange
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);
            var chat = model.StartChat(tools: new List<Tool>());
            var chatInput1 = "What is the weather in Boston?";

            // Act
            //var result1 = await chat.SendMessageStream(prompt);
            //var response1 = await result1.Response;
            //var result2 = await chat.SendMessageStream(new List<IPart> { new FunctionResponse() });
            //var response2 = await result2.Response;

            //// Assert
            //response1.Should().NotBeNull();
            //response.Candidates.Should().NotBeNull().And.HaveCount(1);
            //response.Text.Should().NotBeEmpty();
            //output.WriteLine(response?.Text);
        }

        [Fact(Skip = "Work in progress")]
        public async void Function_Calling_ContentStream()
        {
            // Arrange
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);
            var request = new GenerateContentRequest
            {
                Contents = new List<Content>(),
                Tools = new List<Tool> { }
            };
            request.Contents.Add(new Content
            {
                Role = Role.User,
                Parts = new List<IPart> { new TextData { Text = "What is the weather in Boston?" } }
            });
            request.Contents.Add(new Content
            {
                Role = Role.Model,
                Parts = new List<IPart> { new FunctionCall { Name = "get_current_weather", Args = new { location = "Boston" } } }
            });
            request.Contents.Add(new Content
            {
                Role = Role.Function,
                Parts = new List<IPart> { new FunctionResponse() }
            });

            // Act
            var response = model.GenerateContentStream(request);

            // Assert
            // response.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            // response.FirstOrDefault().Should().NotBeNull();
            // response.ForEach(x => output.WriteLine(x.Text));
            // response.LastOrDefault().UsageMetadata.Should().NotBeNull();
            // output.WriteLine($"PromptTokenCount: {response.LastOrDefault().UsageMetadata.PromptTokenCount}");
            // output.WriteLine($"CandidatesTokenCount: {response.LastOrDefault().UsageMetadata.CandidatesTokenCount}");
            // output.WriteLine($"TotalTokenCount: {response.LastOrDefault().UsageMetadata.TotalTokenCount}");
        }

        [Fact]
        public async void Create_Tuned_Model()
        {
            // Arrange
            var model = new GenerativeModel(apiKey: null, model: Model.Gemini10Pro001)
            {
                AccessToken = fixture.AccessToken,
                ProjectId = fixture.ProjectId
            };
            var request = new CreateTunedModelRequest()
            {
                BaseModel = $"models/{Model.Gemini10Pro001}",
                DisplayName = "Autogenerated Test model",
                TuningTask = new()
                {
                    Hyperparameters = new() { BatchSize = 2, LearningRate = 0.001f, EpochCount = 3 },
                    TrainingData = new()
                    {
                        Examples = new()
                        {
                            Examples = new()
                            {
                                new TrainingDataExample() { TextInput = "1", Output = "2" },
                                new TrainingDataExample() { TextInput = "3", Output = "4" },
                                new TrainingDataExample() { TextInput = "-3", Output = "-2" },
                                new TrainingDataExample() { TextInput = "twenty two", Output = "twenty three" },
                                new TrainingDataExample() { TextInput = "two hundred", Output = "two hundred one" },
                                new TrainingDataExample() { TextInput = "ninety nine", Output = "one hundred" },
                                new TrainingDataExample() { TextInput = "8", Output = "9" },
                                new TrainingDataExample() { TextInput = "-98", Output = "-97" },
                                new TrainingDataExample() { TextInput = "1,000", Output = "1,001" },
                                new TrainingDataExample() { TextInput = "thirteen", Output = "fourteen" },
                                new TrainingDataExample() { TextInput = "seven", Output = "eight" },
                            }
                        }
                    }
                }
            };
            
            // Act
            var response = await model.CreateTunedModel(request);
            
            // Assert
            response.Should().NotBeNull();
            response.Name.Should().NotBeNull();
            response.Metadata.Should().NotBeNull();
            output.WriteLine($"Name: {response.Name}");
            output.WriteLine($"Model: {response.Metadata.TunedModel} (Steps: {response.Metadata.TotalSteps})");
        }

        [Fact]
        public async void Delete_Tuned_Model()
        {
            // Arrange
            var modelName = "tunedModels/number-generator-model-psx3d3gljyko";     // see List_Tuned_Models for available options.
            var model = new GenerativeModel()
            {
                AccessToken = fixture.AccessToken,
                ProjectId = fixture.ProjectId
            };
            
            // Act
            var response = await model.DeleteTunedModel(modelName);
            
            // Assert
            response.Should().NotBeNull();
            output.WriteLine(response);
        }

        [Theory]
        [InlineData("255", "256")]
        [InlineData("41", "42")]
        // [InlineData("five", "six")]
        // [InlineData("Six hundred thirty nine", "Six hundred forty")]
        public async void Generate_Content_TunedModel(string prompt, string expected)
        {
            // Arrange
            var model = new GenerativeModel(apiKey: null, model: "tunedModels/autogenerated-test-model-48gob9c9v54p")
            {
                AccessToken = fixture.AccessToken,
                ProjectId = fixture.ProjectId
            };

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
            response?.Text.Should().Be(expected);
        }
    }
}