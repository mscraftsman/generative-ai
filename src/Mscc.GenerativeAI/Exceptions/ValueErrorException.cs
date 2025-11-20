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

namespace Mscc.GenerativeAI.Types
{
    public class ValueErrorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.ValueErrorException" /> class.
        /// </summary>
        public ValueErrorException() { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.ValueErrorException" /> class 
        /// with a specific message that describes the current exception.
        /// </summary>
        public ValueErrorException(string? message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mscc.GenerativeAI.ValueErrorException" /> class 
        /// with a specific message that describes the current exception and an inner exception.
        /// </summary>
        public ValueErrorException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}