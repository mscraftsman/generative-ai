#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
#endif
#if NET9_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
#endif
using FluentAssertions;
using Mscc.GenerativeAI;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class FunStuffThatShould(ITestOutputHelper output, ConfigurationFixture fixture)
    {
        private readonly string _model = Model.Gemini20FlashThinkingExperimental;

        [Fact]
        public async Task Tell_A_Programming_Joke()
        {
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            var response = await model.GenerateContent(
                prompt: "Tell me a programming joke");
            output.WriteLine(response.Text);
        }

        [Fact]
        public async Task Tell_A_Dad_Joke()
        {
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            var response = await model.GenerateContent(
                prompt: "Tell me a dad joke");
            output.WriteLine(response.Text);
        }
    }
}