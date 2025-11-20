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
	/// <summary>
	/// Google search entry point.
	/// </summary>
	public partial class SearchEntryPoint
	{
		/// <summary>
		/// Optional. Web content snippet that can be embedded in a web page or an app webview.
		/// </summary>
		public string? RenderedContent { get; set; }
		/// <summary>
		/// Optional. Base64 encoded JSON representing array of tuple.
		/// </summary>
		public byte[]? SdkBlob { get; set; }
    }
}