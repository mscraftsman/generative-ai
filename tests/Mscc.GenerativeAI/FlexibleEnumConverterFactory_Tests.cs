using Mscc.GenerativeAI;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Test.Mscc.GenerativeAI
{
    [JsonConverter(typeof(FlexibleEnumConverterFactory))]
    public enum TestEnumWithAttributes
    {
        [EnumMember(Value = "16:9")] 
        Ratio16x9EnumMember,

        [JsonStringEnumMemberName("4:3")]
        Ratio4x3JsonStringEnumMemberName,
        
        // Both attributes, JsonStringEnumMemberName should take precedence ideally, or at least one should work
        [EnumMember(Value = "1:1")]
        [JsonStringEnumMemberName("Square")]
        SquareRatio
    }

    public class FlexibleEnumConverterFactory_Tests
    {
        [Fact]
        public void Serialize_EnumWithAttributes_UseAttributeValue()
        {
            // Arrange
            var options = new JsonSerializerOptions
            {
                Converters = { new FlexibleEnumConverterFactory() }
            };

            // Act & Assert
            // Case 1: EnumMemberAttribute
            var expected16x9 = "\"16:9\"";
            var result16x9 = JsonSerializer.Serialize(TestEnumWithAttributes.Ratio16x9EnumMember, options);
            Assert.Equal(expected16x9, result16x9);

            // Case 2: JsonStringEnumMemberNameAttribute
            var expected4x3 = "\"4:3\"";
            var result4x3 = JsonSerializer.Serialize(TestEnumWithAttributes.Ratio4x3JsonStringEnumMemberName, options);
            Assert.Equal(expected4x3, result4x3);

             // Case 3: Both (JsonStringEnumMemberName usually preferred in System.Text.Json, let's verify both are checked)
            var expectedSquare = "\"Square\""; 
            var resultSquare = JsonSerializer.Serialize(TestEnumWithAttributes.SquareRatio, options);
            Assert.Equal(expectedSquare, resultSquare);
        }
    }
}
