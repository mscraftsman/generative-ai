using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Google;
using Mscc.GenerativeAI.Types;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI.Google
{
    [Collection(nameof(ConfigurationFixture))]
    public class GenerativeModelGoogle_Should(ITestOutputHelper output, ConfigurationFixture fixture)
    {
        private readonly string _model = Model.Gemini25Pro;

        [Fact]
        public void Initiate()
        {
            // Arrange
            // Act
            var vertex = new GenerativeModelGoogle();

            // Assert
            vertex.ShouldNotBeNull();
        }

        [Fact]
        public void Initiate_Static_OAuth()
        {
            // Arrange
            // Act
            GenerativeModelGoogle vertex = GenerativeModelGoogle.CreateInstance();

            // Assert
            vertex.ShouldNotBeNull();
        }

        [Fact]
        public void Initiate_Static_ServiceAccount()
        {
            // Arrange
            //var serviceAccount = "";
            
            // Act
            var vertex = GenerativeModelGoogle.CreateInstance(fixture.ServiceAccount, passphrase:fixture.Passphrase);
            
            // Assert
            vertex.ShouldNotBeNull();
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
            vertex.ShouldNotBeNull();
        }

        [Fact]
        public void Initiate_With_ServiceAccount()
        {
            // Arrange & Act
            var vertex = GenerativeModelGoogle.CreateInstance(fixture.ServiceAccount, passphrase:fixture.Passphrase)
                .WithProjectId(fixture.ProjectId)
                .WithRegion(fixture.Region);
        
            // Assert
            vertex.ShouldNotBeNull();
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
            model.ShouldNotBeNull();
            model.Name.ShouldBe(Model.Gemini25Pro.SanitizeModelName());
        }

        [Theory]
        [InlineData(Model.Gemini25Pro)]
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
            model.ShouldNotBeNull();
            model.Name.ShouldBe(expected.SanitizeModelName());
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