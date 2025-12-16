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

namespace Mscc.GenerativeAI.Types
{
    internal struct Constants
    {
        internal const uint ChunkSize = 8388608;
	    internal const int EmbeddingDimensions = 768;
        internal const long MaxUploadFileSize = 2147483648;
        internal const long MaxUploadFileSizeRagStore = 104857600;
        internal const long MaxUploadFileSizeFileSearchStore = 104857600;
        internal const string MediaType = "application/json";
        internal const string RequestFailed = "Request failed";
        internal const string RequestFailedWithStatusCode = "Request failed with Status Code: ";
        internal static readonly int[] RetryStatusCodes = [408, 429, 500, 502, 503, 504];
    }
}