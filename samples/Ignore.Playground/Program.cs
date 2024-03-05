using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

var accessToken = string.Empty;
var testing = new Testing();
if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
{
    accessToken = testing.RunExternalExe("cmd.exe", "/c gcloud auth application-default print-access-token").TrimEnd();
}
else
{
    accessToken = testing.RunExternalExe("gcloud", "auth application-default print-access-token").TrimEnd();
}

Console.WriteLine($"OS: {System.Runtime.InteropServices.RuntimeInformation.OSDescription}");
Console.WriteLine($"Access Token: {accessToken}");

public class Testing
{
    public string RunExternalExe(string filename, string arguments = null)
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
            throw new Exception("OS error while executing " + Format(filename, arguments) + ": " + e.Message, e);
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
}