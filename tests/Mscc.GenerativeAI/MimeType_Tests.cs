using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace Test.Mscc.GenerativeAI
{
    public class MimeTypeTests
    {
        [Theory]
        [InlineData("test.png", "image/png")]
        [InlineData("test.PNG", "image/png")] // Case-insensitivity
        [InlineData("test.jpeg", "image/jpeg")]
        [InlineData("test.jpg", "image/jpeg")]
        [InlineData("test.pdf", "application/pdf")]
        [InlineData("test.txt", "text/plain")]
        [InlineData("test.323", "text/h323")] // Obscure type in switch statement
        [InlineData("test.unknownextension", "application/octet-stream")] // Default fallback
        public void GetMimeType_Should_Return_Expected_MimeType(string filename, string expectedMimeType)
        {
            // Act
            var result = GenerativeAIExtensions.GetMimeType(filename);

            // Assert
            result.ShouldBe(expectedMimeType);
        }

        [Fact]
        public void GuardMimeTypeFileSearchStore_Should_Allow_Valid_MimeTypes()
        {
            var logger = new TestLogger();

            // Act & Assert (Should not throw and not log warnings)
            Should.NotThrow(() => GenerativeAIExtensions.GuardMimeTypeFileSearchStore("application/pdf", logger));
            logger.LoggedMessage.ShouldBeNull();
        }

        [Fact]
        public void GuardMimeTypeFileSearchStore_Should_Log_Warning_On_Unverified_MimeTypes()
        {
            var logger = new TestLogger();

            // Act
            GenerativeAIExtensions.GuardMimeTypeFileSearchStore("image/invalid-for-search-store", logger);

            // Assert
            logger.LoggedMessage.ShouldNotBeNull();
            logger.LoggedLevel.ShouldBe(LogLevel.Warning);
            logger.LoggedMessage.ShouldContain("image/invalid-for-search-store");
        }

        [Fact]
        public void GuardMimeType_Should_Log_Warning_On_Unverified_MimeTypes()
        {
            var logger = new TestLogger();

            // Act
            GenerativeAIExtensions.GuardMimeType("application/octet-stream-unknown", logger);

            // Assert
            logger.LoggedMessage.ShouldNotBeNull();
            logger.LoggedLevel.ShouldBe(LogLevel.Warning);
            logger.LoggedMessage.ShouldContain("application/octet-stream-unknown");
        }

        [Fact]
        public void GuardMimeType_Should_Not_Log_Warning_On_Verified_MimeTypes()
        {
            var logger = new TestLogger();

            // Act
            GenerativeAIExtensions.GuardMimeType("image/png", logger);

            // Assert
            logger.LoggedMessage.ShouldBeNull();
        }

        [Fact]
        public void GuardMimeType_With_Null_Logger_Should_Execute_Without_Throwing()
        {
            // Act & Assert (Should not throw)
            Should.NotThrow(() => GenerativeAIExtensions.GuardMimeType("application/unknown-mime-type", null));
            Should.NotThrow(() => GenerativeAIExtensions.GuardMimeTypeFileSearchStore("application/unknown-mime-type", null));
        }

        [Theory]
        [InlineData("IMAGE/PNG")]
        [InlineData("image/Png")]
        [InlineData("APPLICATION/PDF")]
        public void GuardMimeType_Should_Be_Case_Insensitive(string caseVariedMimeType)
        {
            var logger = new TestLogger();

            // Act
            GenerativeAIExtensions.GuardMimeType(caseVariedMimeType, logger);

            // Assert
            logger.LoggedMessage.ShouldBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void GuardMimeType_With_Null_Or_Empty_MimeType_Should_Log_Warning_Gracefully(string? edgeMimeType)
        {
            var logger = new TestLogger();

            // Act
            GenerativeAIExtensions.GuardMimeType(edgeMimeType!, logger);

            // Assert
            logger.LoggedMessage.ShouldNotBeNull();
            logger.LoggedLevel.ShouldBe(LogLevel.Warning);
        }

        private class TestLogger : ILogger
        {
            public string? LoggedMessage { get; private set; }
            public LogLevel LoggedLevel { get; private set; }

            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                LoggedLevel = logLevel;
                LoggedMessage = formatter(state, exception);
            }
        }
    }
}
