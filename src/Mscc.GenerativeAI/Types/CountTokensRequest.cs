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

using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Request for counting tokens.
	/// </summary>
	public partial class CountTokensRequest : IVertexAware
	{
		/// <summary>
		/// Configuration for counting tokens.
		/// </summary>
		public CountTokensConfig? Config { get; set; }

		public void PrepareForSerialization(bool useVertexAi)
		{
			if (useVertexAi)
			{
				if (Contents != null && Instances == null)
				{
					Instances = new List<object>(Contents);
					Contents = null;
				}
			}
			else
			{
				if (Instances != null && Contents == null)
				{
					Contents = new List<Content>();
					foreach (var instance in Instances)
					{
						if (instance is Content content)
						{
							Contents.Add(content);
						}
					}
					Instances = null;
				}
			}
		}
	}
}