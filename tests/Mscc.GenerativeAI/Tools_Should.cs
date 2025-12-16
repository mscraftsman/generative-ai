using Mscc.GenerativeAI.Types;
using Shouldly;
using System.Collections.Generic;
using Xunit;

namespace Test.Mscc.GenerativeAI
{
    public class Tools_Should
    {
        [Fact]
        public void Invoke_Function_Given_Delegate()
        {
            // Arrange
            var tools = new Tools();
            tools.AddFunction(GetCurrentWeather);

            var arguments = new Dictionary<string, object>
            {
                { "location", "London" },
                { "unit", "celsius" }
            };

            // Act
            var result = tools.Invoke("GetCurrentWeather", arguments);

            // Assert
            result.ShouldBe("The weather in London is 15 degrees celsius.");
        }

        private string GetCurrentWeather(string location, string unit = "celsius")
        {
            return $"The weather in {location} is 15 degrees {unit}.";
        }
    }
}
