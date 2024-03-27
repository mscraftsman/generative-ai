﻿#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.IO;
#endif
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Text;
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
        public string ServiceAccount { get; set; }


        // Todo: Handle envVar GOOGLE_APPLICATION_CREDENTIALS
        // Ref: https://cloud.google.com/vertex-ai/docs/start/client-libraries
        public ConfigurationFixture()
        {
            ReadDotEnv();

            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.user.json", optional: true)
                .AddJsonFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "gcloud",
                        "application_default_credentials.json"), optional: true)
                .AddUserSecrets("b2e3f388-3f81-435b-9467-73081761ab6f") // Add User Secrets
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
            if (string.IsNullOrEmpty(ApiKey))
            {
                AccessToken = Configuration["access_token"];
                if (string.IsNullOrEmpty(AccessToken))
                    AccessToken = Environment.GetEnvironmentVariable("GOOGLE_ACCESS_TOKEN");
                if (string.IsNullOrEmpty(AccessToken))
                {
                    if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices
                            .OSPlatform.Windows))
                    {
                        AccessToken = RunExternalExe("cmd.exe", "/c gcloud auth application-default print-access-token")
                            .TrimEnd();
                    }
                    else
                    {
                        AccessToken = RunExternalExe("gcloud", "auth application-default print-access-token").TrimEnd();
                    }
                }
            }

            ServiceAccount = Configuration["service_account"];
        }

        private string RunExternalExe(string filename, string arguments = null)
        {
            var process = new Process();

            process.StartInfo.FileName = filename;
            if (!string.IsNullOrEmpty(arguments))
            {
                process.StartInfo.Arguments = arguments;
            }

            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;

            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            var stdOutput = new StringBuilder();
            process.OutputDataReceived += (sender, args) => stdOutput.AppendLine(args.Data); // Use AppendLine rather than Append since args.Data is one line of output, not including the newline character.

            string stdError = null;
            try
            {
                process.Start();
                process.BeginOutputReadLine();
                stdError = process.StandardError.ReadToEnd();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                throw new Exception("OS error while executing " + Format(filename, arguments)+ ": " + e.Message, e);
            }

            if (process.ExitCode == 0)
            {
                return stdOutput.ToString();
            }
            else
            {
                var message = new StringBuilder();

                if (!string.IsNullOrEmpty(stdError))
                {
                    message.AppendLine(stdError);
                }

                if (stdOutput.Length != 0)
                {
                    message.AppendLine("Std output:");
                    message.AppendLine(stdOutput.ToString());
                }

                throw new Exception(Format(filename, arguments) + " finished with exit code = " + process.ExitCode + ": " + message);
            }
        }

        private string Format(string filename, string arguments)
        {
            return "'" + filename + 
                ((string.IsNullOrEmpty(arguments)) ? string.Empty : " " + arguments) +
                "'";
        }

        private void ReadDotEnv(string dotEnvFile = ".env")
        {
            if (!File.Exists(dotEnvFile)) return;

            foreach (var line in File.ReadAllLines(dotEnvFile))
            {
                var parts = line.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2) continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }    
    }
}