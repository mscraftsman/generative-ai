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
	/// Probe describes a health check to be performed against a container to determine whether it is alive or ready to receive traffic.
	/// </summary>
	public partial class Probe
	{
		/// <summary>
		/// ExecAction probes the health of a container by executing a command.
		/// </summary>
		public ProbeExecAction? Exec { get; set; }
		/// <summary>
		/// Number of consecutive failures before the probe is considered failed. Defaults to 3. Minimum value is 1. Maps to Kubernetes probe argument &apos;failureThreshold&apos;.
		/// </summary>
		public int? FailureThreshold { get; set; }
		/// <summary>
		/// GrpcAction probes the health of a container by sending a gRPC request.
		/// </summary>
		public ProbeGrpcAction? Grpc { get; set; }
		/// <summary>
		/// HttpGetAction probes the health of a container by sending an HTTP GET request.
		/// </summary>
		public ProbeHttpGetAction? HttpGet { get; set; }
		/// <summary>
		/// Number of seconds to wait before starting the probe. Defaults to 0. Minimum value is 0. Maps to Kubernetes probe argument &apos;initialDelaySeconds&apos;.
		/// </summary>
		public int? InitialDelaySeconds { get; set; }
		/// <summary>
		/// How often (in seconds) to perform the probe. Default to 10 seconds. Minimum value is 1. Must be less than timeout_seconds. Maps to Kubernetes probe argument &apos;periodSeconds&apos;.
		/// </summary>
		public int? PeriodSeconds { get; set; }
		/// <summary>
		/// Number of consecutive successes before the probe is considered successful. Defaults to 1. Minimum value is 1. Maps to Kubernetes probe argument &apos;successThreshold&apos;.
		/// </summary>
		public int? SuccessThreshold { get; set; }
		/// <summary>
		/// TcpSocketAction probes the health of a container by opening a TCP socket connection.
		/// </summary>
		public ProbeTcpSocketAction? TcpSocket { get; set; }
		/// <summary>
		/// Number of seconds after which the probe times out. Defaults to 1 second. Minimum value is 1. Must be greater or equal to period_seconds. Maps to Kubernetes probe argument &apos;timeoutSeconds&apos;.
		/// </summary>
		public int? TimeoutSeconds { get; set; }
    }
}