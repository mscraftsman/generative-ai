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

using System.Diagnostics;

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A safety setting that affects the safety-blocking behavior. A SafetySetting consists of a harm
	/// category and a threshold for that category.
	/// </summary>
	/// <remarks>
	/// Represents a safety setting that can be used to control the model's behavior.
	/// It instructs the model to avoid certain responses given safety measurements based on category.
	/// Ref: https://ai.google.dev/api/rest/v1beta/SafetySetting
	/// </remarks>
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public partial class SafetySetting
	{
		private string GetDebuggerDisplay()
		{
			return $"Category: {Category} - Threshold: {Threshold}";
		}
	}
}