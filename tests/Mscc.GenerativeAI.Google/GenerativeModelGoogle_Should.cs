#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Linq;
#endif
#if NET9_0
using System;
using System.Threading.Tasks;
#endif
using FluentAssertions;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Google;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI.Google
{
    [Collection(nameof(ConfigurationFixture))]
    public class GenerativeModelGoogle_Should(ITestOutputHelper output, ConfigurationFixture fixture)
    {
        private readonly string _model = Model.Gemini15Pro;

        [Fact]
        public void Initiate()
        {
            // Arrange
            // Act
            var vertex = new GenerativeModelGoogle();

            // Assert
            vertex.Should().NotBeNull();
        }

        [Fact]
        public void Initiate_Static_OAuth()
        {
            // Arrange
            // Act
            GenerativeModelGoogle vertex = GenerativeModelGoogle.CreateInstance();

            // Assert
            vertex.Should().NotBeNull();
        }

        [Fact]
        public void Initiate_Static_ServiceAccount()
        {
            // Arrange
            //var serviceAccount = "";
            
            // Act
            var vertex = GenerativeModelGoogle.CreateInstance(fixture.ServiceAccount, passphrase:fixture.Passphrase);
            
            // Assert
            vertex.Should().NotBeNull();
        }

        [Fact]
        public void Initiate_With_OAuth()
        {
            // Arrange
            // Act
            var vertex = new GenerativeModelGoogle()
            {
                ProjectId = fixture.ProjectId, 
                Region = fixture.Region
            };

            // Assert
            vertex.Should().NotBeNull();
        }

        [Fact]
        public void Initiate_With_ServiceAccount()
        {
            // Arrange & Act
            var vertex = GenerativeModelGoogle.CreateInstance(fixture.ServiceAccount, passphrase:fixture.Passphrase)
                .WithProjectId(fixture.ProjectId)
                .WithRegion(fixture.Region);
        
            // Assert
            vertex.Should().NotBeNull();
        }

        [Fact]
        public void Initiate_Default_Model()
        {
            // Arrange
            var vertex = new GenerativeModelGoogle()
            {
                ProjectId = fixture.ProjectId, 
                Region = fixture.Region
            };
            
            // Act
            var model = vertex.CreateModel();

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be(Model.Gemini15Pro.SanitizeModelName());
        }

        [Theory]
        [InlineData(Model.Gemini10ProVision)]
        [InlineData(Model.Gemini15Pro)]
        [InlineData(Model.Gemini20Pro)]
        public void Initiate_Model(string expected)
        {
            // Arrange
            var vertex = new GenerativeModelGoogle()
            {
                ProjectId = fixture.ProjectId, 
                Region = fixture.Region
            };
            
            // Act
            var model = vertex.CreateModel(expected);

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be(expected.SanitizeModelName());
        }

        [Fact]
        public async Task List_Models()
        {
            // Arrange
            var vertex = new GenerativeModelGoogle()
            {
                ProjectId = fixture.ProjectId, 
                Region = fixture.Region
            };
            var model = vertex.CreateModel(model: _model);

            // Act & Assert
            await Assert.ThrowsAsync<NotSupportedException>(() => model.ListModels());
        }

        [Fact]
        public async Task List_Models_With_ServiceAccount()
        {
            // Arrange
            var vertex = GenerativeModelGoogle.CreateInstance(fixture.ServiceAccount, passphrase:fixture.Passphrase)
                .WithProjectId(fixture.ProjectId)
                .WithRegion(fixture.Region);
            var model = vertex.CreateModel(model: _model);

            // Act & Assert
            await Assert.ThrowsAsync<NotSupportedException>(() => model.ListModels());
        }
    }
}