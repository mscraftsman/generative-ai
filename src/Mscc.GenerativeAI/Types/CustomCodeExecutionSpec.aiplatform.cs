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

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Specificies a metric that is populated by evaluating user-defined Python code.
	/// </summary>
	public partial class CustomCodeExecutionSpec
	{
		/// <summary>
		/// Required. Python function. Expected user to define the following function, e.g.: def evaluate(instance: dict[str, Any]) -&gt; float: Please include this function signature in the code snippet. Instance is the evaluation instance, any fields populated in the instance are available to the function as instance[field_name]. Example: Example input: ``<c> instance= EvaluationInstance( response=EvaluationInstance.InstanceData(text=&quot;The answer is 4.&quot;), reference=EvaluationInstance.InstanceData(text=&quot;4&quot;) ) </c>`<c> Example converted input: </c>`<c> { &apos;response&apos;: {&apos;text&apos;: &apos;The answer is 4.&apos;}, &apos;reference&apos;: {&apos;text&apos;: &apos;4&apos;} } </c>`<c> Example python function: </c>`<c> def evaluate(instance: dict[str, Any]) -&gt; float: if instance&apos;response&apos; == instance&apos;reference&apos;: return 1.0 return 0.0 </c>`` CustomCodeExecutionSpec is also supported in Batch Evaluation (EvalDataset RPC) and Tuning Evaluation. Each line in the input jsonl file will be converted to dict[str, Any] and passed to the evaluation function.
		/// </summary>
		public string? EvaluationFunction { get; set; }
    }
}