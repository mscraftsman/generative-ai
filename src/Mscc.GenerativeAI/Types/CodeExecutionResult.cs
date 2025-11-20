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
	/// Result of executing the <see cref="ExecutableCode"/>. Only generated when using the <see cref="CodeExecution"/>, and always follows a <see cref="part"/> containing the <see cref="ExecutableCode"/>.
	/// </summary>
	public partial class CodeExecutionResult
	{
		/// <summary>
		/// Required. Outcome of the code execution.
		/// </summary>
		public Outcome? Outcome { get; set; }
		/// <summary>
		/// Optional. Contains stdout when code execution is successful, stderr or other description otherwise.
		/// </summary>
		public string? Output { get; set; }
    }
}