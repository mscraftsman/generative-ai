using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

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
        /// Logs <see cref="GenerativeModel"/>
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        [LoggerMessage(EventId = 0, Level = LogLevel.Debug, Message = "Generative model starting")]
        public static partial void LogGenerativeModelInvoking(
            this ILogger logger);

        /// <summary>
        /// Logs <see cref="GenerativeModel"/>
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "Generative model started")]
        public static partial void LogGenerativeModelInvoked(
            this ILogger logger);
        
        /// <summary>
        /// Logs <see cref="GenerativeModel"/> invoking an API request.
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        /// <param name="methodName">Calling method</param>
        /// <param name="url">URL of Gemini API endpoint</param>
        /// <param name="payload">Data sent to the API endpoint</param>
        [LoggerMessage(EventId = 0, Level = LogLevel.Debug, Message = "[{MethodName}] Request: {Url} - {Payload}")]
        public static partial void LogGenerativeModelInvokingRequest(
            this ILogger logger,
            string methodName,
            string url,
            string payload);

        /// <summary>
        /// Logs <see cref="BaseModel"/> when exception thrown to run an external application.
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        /// <param name="message">Message of <see cref="System.Exception"/> to log.</param>
        [LoggerMessage(EventId = 0, Level = LogLevel.Warning, Message = "{Message}")]
        public static partial void LogRunExternalExe(
            this ILogger logger,
            string message);

        /// <summary>
        /// Logs <see cref="BaseModel"/> parsing the URL to call.
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        /// <param name="url">Parsed URL.</param>
        [LoggerMessage(EventId = 0, Level = LogLevel.Debug, Message = "Request URL: {Url}")]
        public static partial void LogParsedRequestUrl(
            this ILogger logger,
            string url);

        [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "Request JSON: {json}")]
        public static partial void LogJsonRequest(
            this ILogger logger,
            string json);

        [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "Response JSON: {json}")]
        public static partial void LogJsonResponse(
            this ILogger logger,
            string json);
    }
}