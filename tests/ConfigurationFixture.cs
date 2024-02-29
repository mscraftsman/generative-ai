using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using Xunit;

namespace Test.Mscc.GenerativeAI
{
    [CollectionDefinition(nameof(ConfigurationFixture))]
    public class ConfigurationFixture : ICollectionFixture<ConfigurationFixture>
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
            if (string.IsNullOrEmpty(ApiKey))
                ApiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
            ProjectId = Configuration["project_id"];
            if (string.IsNullOrEmpty(ProjectId))
                ProjectId = Environment.GetEnvironmentVariable("GOOGLE_PROJECT_ID");
            Region = Configuration["region"];
            if (string.IsNullOrEmpty(Region))
                Region = Environment.GetEnvironmentVariable("GOOGLE_REGION");
            AccessToken = Configuration["access_token"];
            if (string.IsNullOrEmpty(AccessToken))
                AccessToken = Environment.GetEnvironmentVariable("GOOGLE_ACCESS_TOKEN");
            if (string.IsNullOrEmpty(AccessToken))
                AccessToken = ReadAccessToken().TrimEnd();
        }

        private string ReadAccessToken()
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c gcloud auth application-default print-access-token";
            p.Start();
            var output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            return output;
        }
    }
}