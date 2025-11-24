/*
 * Copyleft 2024-2025 Jochen Kirstätter and contributors
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

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// ExecAction specifies a command to execute.
	/// </summary>
	public partial class ProbeExecAction
	{
		/// <summary>
		/// Command is the command line to execute inside the container, the working directory for the command is root (&apos;/&apos;) in the container&apos;s filesystem. The command is simply exec&apos;d, it is not run inside a shell, so traditional shell instructions (&apos;|&apos;, etc) won&apos;t work. To use a shell, you need to explicitly call out to that shell. Exit status of 0 is treated as live/healthy and non-zero is unhealthy.
		/// </summary>
		public List<string>? Command { get; set; }
    }
}