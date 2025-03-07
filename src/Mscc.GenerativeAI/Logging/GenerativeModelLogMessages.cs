using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Mscc.GenerativeAI
{
    #pragma warning disable SYSLIB1006 // Multiple logging methods cannot use the same event id within a class
    
    /// <summary>
    /// Extensions for logging <see cref="GenerativeModel"/> invocations.
    /// </summary>
    /// <remarks>
    /// This extension uses the <see cref="LoggerMessageAttribute"/> to
    /// generate logging code at compile time to achieve optimized code.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    internal static partial class GenerativeModelLogMessages
    {
        /// <summary>
        /// Logs <see cref="BaseModel"/> parsing the URL to call.
        /// </summary>
        /// <param name="logger">Logger instance used for logging</param>
        /// <param name="method">HTTP method of the request.</param>
        /// <param name="uri">Parsed URL.</param>
        /// <param name="headers"></param>
        /// <param name="content"></param>
        [LoggerMessage(EventId = 0, Level = LogLevel.Trace, Message = "Request URL: \n{Method} {Uri}\n{Headers}\n{Content}")]
        public static partial void LogParsedRequest(
            this ILogger logger,
            HttpMethod method,
            Uri? uri,
            string? headers,
            string? content);
        
        /// <summary>
        /// Logs <see cref="GenerativeModel"/> invoking an API request.
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        /// <param name="methodName">Calling method</param>
        /// <param name="url">URL of Gemini API endpoint</param>
        /// <param name="payload">Data sent to the API endpoint</param>
        [LoggerMessage(EventId = 0, Level = LogLevel.Debug, Message = "[{MethodName}]")]
        public static partial void LogMethodInvokingRequest(
            this ILogger logger,
            string methodName);

        [LoggerMessage(EventId = 0, Level = LogLevel.Debug, Message = "Request JSON: \n{json}")]
        public static partial void LogJsonRequest(
            this ILogger logger,
            string json);

        [LoggerMessage(EventId = 0, Level = LogLevel.Debug, Message = "Response JSON: \n{json}")]
        public static partial void LogJsonResponse(
            this ILogger logger,
            string json);

        /// <summary>
        /// Logs <see cref="GenerativeModel"/>
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "Generative model starting")]
        public static partial void LogGenerativeModelInvoking(
            this ILogger logger);

        /// <summary>
        /// Logs <see cref="GenerativeModel"/>
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "Generative model started")]
        public static partial void LogGenerativeModelInvoked(
            this ILogger logger);

        [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message =
            "There are {count} Candidates, returning text from the first candidate. Access response.Candidates directly to get text from other candidates.")]
        public static partial void LogMultipleCandidates(
            this ILogger logger,
            int count);

        /// <summary>
        /// Logs <see cref="BaseModel"/> when exception thrown to run an external application.
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        /// <param name="message">Message of <see cref="System.Exception"/> to log.</param>
        [LoggerMessage(EventId = 0, Level = LogLevel.Warning, Message = "{Message}")]
        public static partial void LogRunExternalExe(
            this ILogger logger,
            string message);
    }
}