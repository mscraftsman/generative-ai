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
using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class FunStuffThatShould(ITestOutputHelper output, ConfigurationFixture fixture)
    {
        private readonly string _model = Model.Gemini20FlashThinkingExperimental;

        [Theory]
        [InlineData("Tell me a programming joke")]
        [InlineData("Tell me a dad joke")]
        public async Task Tell_A_Joke(string prompt)
        {
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            var response = await model.GenerateContent(
                prompt: prompt);
            output.WriteLine(response.Text);
        }

        [Theory]
        [InlineData("Tell me about De Morgan in Boolean Algebra")]
        public async Task Tell_Something_About_Mathematics(string prompt)
        {
            var systemInstruction = new Content(@"You are an expert mathematics professor. 
Give you response some thought and explain certain prompts in more details than the usual answer.
In case that you are providing mathematical formula and equations, then use LaTeX syntax with inline mode");
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model, 
                systemInstruction: systemInstruction);
            var response = await model.GenerateContent(prompt: prompt);
            output.WriteLine(response.Text);
        }
    }
}