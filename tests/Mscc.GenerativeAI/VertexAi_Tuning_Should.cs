using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Types;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class VertexAiTuningShould
    {
        private readonly ITestOutputHelper output;
        private readonly ConfigurationFixture fixture;
        private readonly string _model = Model.Gemini20Flash001;
        private readonly VertexAI _genAi;
        private readonly SupervisedTuningJobModel _tuningJob;

        public VertexAiTuningShould(ITestOutputHelper output, ConfigurationFixture fixture)
        {
            this.output = output;
            this.fixture = fixture;
            _genAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);
            _tuningJob = _genAi.SupervisedTuningJob();
        }

        [Fact]
        public void Initialize_VertexAI()
        {
            // Arrange

            // Act
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);

            // Assert
            vertexAi.ShouldNotBeNull();
        }

        [Fact]
        public void Initialize_Using_VertexAI()
        {
            // Arrange
            var expected = Model.Gemini20Flash001;
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);

            // Act
            var model = vertexAi.SupervisedTuningJob();
            
            // Assert
            model.ShouldNotBeNull();
            model.Name.ShouldBe($"{expected.SanitizeModelName()}");
        }
        
        [Fact]
        public async Task Initialize_Should_Throw_ArgumentException()
        {
            // Arrange
            // Act
            Func<Task> action = async () =>
            {
                var request = new CreateTuningJobRequest(_model, "");
            };
            
            // Assert
            var exception = await action.ShouldThrowAsync<ArgumentException>();
            exception.Message.ShouldMatch("Value cannot be null or empty.*");
        }
        
        [Fact]
        public async Task Create_Should_Throw_ArgumentNullException_WhenRequestIsNull()
        {
            // Act
            Func<Task> action = async () => await _tuningJob.Create(request: null);

            // Assert
            var exception = await action.ShouldThrowAsync<ArgumentNullException>();
            exception.ParamName.ShouldBe("request");
        }
        
        [Fact]
        public async Task Create_Should_Throw_HttpRequestException_WhenWrong()
        {
            // Arrange
            var request = new CreateTuningJobRequest(_model, "gs://");
            
            // Act
            Func<Task> action = async () => await _tuningJob.Create(request);
            
            // Assert
            await action.ShouldThrowAsync<HttpRequestException>();
        }
        
        [Fact]
        public async Task Create_TuningJob()
        {
            // Arrange
            var request = new CreateTuningJobRequest(_model,
                datasetUri: "gs://cloud-samples-data/ai-platform/generative_ai/gemini-1_5/text/sft_train_data.jsonl",
                validationUri: "gs://cloud-samples-data/ai-platform/generative_ai/gemini-1_5/text/sft_validation_data.jsonl",
                displayName: "tuned_gemini_1_5_pro",
                new Hyperparameters() { 
                    EpochCount = 4, 
                    AdapterSize = AdapterSize.AdapterSizeFour, 
                    LearningRateMultiplier = 1.0f
                });
            
            // Act
            var result =  await _tuningJob.Create(request);
            
            // Assert
            result.ShouldNotBeNull();
            result.Name.ShouldNotBeNull();
        }

        [Fact]
        public async Task List_TuningJobs()
        {
            // Act
            var result = await _tuningJob.List();

            // Assert
            result.ShouldNotBeNull();
            foreach (TuningJob item in result)
            {
                output.WriteLine($"{item.TunedModelDisplayName}");
                output.WriteLine($"Completed: {item.HasEnded} - {Enum.GetName(typeof(StateTuningJob), item.State)}");
                output.WriteLine($"{item.Name} ({item.CreateTime})");
            }
        }

        [Theory]
        [InlineData("projects/646763706049/locations/us-central1/tuningJobs/6284765961371254784")]
        public async Task Get_TuningJob(string name)
        {
            // Arrange

            // Act
            var result = await _tuningJob.Get(name);

            // Assert
            result.ShouldNotBeNull();
            result.Name.ShouldBe(name);
            output.WriteLine($"Completed: {result.HasEnded} - {Enum.GetName(typeof(TuningJob.StateType), result.State)}");
            output.WriteLine($"{result.Name} ({result.CreateTime})");
        }

        [Fact]
        public async Task Cancel_TuningJob()
        {
            // Arrange
            var tuningJobs = await _tuningJob.List();
            var name = tuningJobs.FirstOrDefault()?.Name;
            output.WriteLine(name);
            
            // Act
            var response = await _tuningJob.Cancel(name);
            
            // Assert
            response.ShouldNotBeNull();
            output.WriteLine(response);
        }

        [Fact(Skip = "Method not found.")]
        public async Task Delete_TuningJob()
        {
            // Arrange
            var tuningJobs = await _tuningJob.List();
            var name = tuningJobs.FirstOrDefault()?.Name;
            output.WriteLine(name);
            
            // Act
            var response = await _tuningJob.Delete(name);
            
            // Assert
            response.ShouldNotBeNull();
            output.WriteLine(response);
        }
    }
}