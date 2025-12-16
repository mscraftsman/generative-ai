/*
 * Copyright 2024-2025 Jochen Kirstätter
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      https://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Net.Http;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Represents errors that occur during Generative AI API calls.
    /// </summary>
    public class GeminiApiTimeoutException : Exception
    {
        /// <summary>
        /// HTTP response from the API.
        /// </summary>
        public HttpResponseMessage? Response { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeminiApiTimeoutException"/> class.
        /// </summary>
        public GeminiApiTimeoutException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeminiApiTimeoutException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public GeminiApiTimeoutException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeminiApiTimeoutException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public GeminiApiTimeoutException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeminiApiTimeoutException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="response">The HTTP response.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public GeminiApiTimeoutException(string message, HttpResponseMessage response, Exception? innerException = null)
            : base(message, innerException)
        {
            Response = response;
        }
    }
}
