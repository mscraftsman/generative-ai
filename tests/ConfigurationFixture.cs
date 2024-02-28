using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xunit;

namespace Test.Mscc.GenerativeAI
{
    public class ConfigurationFixture : IDisposable
    {
        private IConfiguration Configuration { get; }

        public string ApiKey { get; set; } = default;
        public string ProjectId { get; set; } = default;
        public string Region { get; set; } = default;
        public string AccessToken { get; set; } = default;


        public ConfigurationFixture()
        {
            Configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true)
               .AddJsonFile("appsettings.user.json", optional: true)
               .AddJsonFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "gcloud", "application_default_credentials.json"), optional: true)
               .AddEnvironmentVariables()
               .Build();

            ApiKey = Configuration["api_key"];
            ProjectId = Configuration["project_id"];
            Region = Configuration["region"];
            AccessToken = Configuration["access_token"];
        }

        public void Dispose()
        {
        }
    }

    [CollectionDefinition("Configuration")]
    public class ConfigurationCollection : ICollectionFixture<ConfigurationFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}