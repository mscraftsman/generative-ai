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
	/// Specification of a container for serving predictions. Some fields in this message correspond to fields in the [Kubernetes Container v1 core specification](https://kubernetes.io/docs/reference/generated/kubernetes-api/v1.23/#container-v1-core).
	/// </summary>
	public partial class ModelContainerSpec
	{
		/// <summary>
		/// Immutable. Specifies arguments for the command that runs when the container starts. This overrides the container&apos;s [<c>CMD</c>](https://docs.docker.com/engine/reference/builder/#cmd). Specify this field as an array of executable and arguments, similar to a Docker <c>CMD</c>&apos;s &quot;default parameters&quot; form. If you don&apos;t specify this field but do specify the command field, then the command from the <c>command</c> field runs without any additional arguments. See the [Kubernetes documentation about how the <c>command</c> and <c>args</c> fields interact with a container&apos;s <c>ENTRYPOINT</c> and <c>CMD</c>](https://kubernetes.io/docs/tasks/inject-data-application/define-command-argument-container/#notes). If you don&apos;t specify this field and don&apos;t specify the <c>command</c> field, then the container&apos;s [<c>ENTRYPOINT</c>](https://docs.docker.com/engine/reference/builder/#cmd) and <c>CMD</c> determine what runs based on their default behavior. See the Docker documentation about [how <c>CMD</c> and <c>ENTRYPOINT</c> interact](https://docs.docker.com/engine/reference/builder/#understand-how-cmd-and-entrypoint-interact). In this field, you can reference [environment variables set by Vertex AI](https://cloud.google.com/vertex-ai/docs/predictions/custom-container-requirements#aip-variables) and environment variables set in the env field. You cannot reference environment variables set in the Docker image. In order for environment variables to be expanded, reference them by using the following syntax: $( VARIABLE_NAME) Note that this differs from Bash variable expansion, which does not use parentheses. If a variable cannot be resolved, the reference in the input string is used unchanged. To avoid variable expansion, you can escape this syntax with <c>$$</c>; for example: $$(VARIABLE_NAME) This field corresponds to the <c>args</c> field of the Kubernetes Containers [v1 core API](https://kubernetes.io/docs/reference/generated/kubernetes-api/v1.23/#container-v1-core).
		/// </summary>
		public List<string>? Args { get; set; }
		/// <summary>
		/// Immutable. Specifies the command that runs when the container starts. This overrides the container&apos;s [ENTRYPOINT](https://docs.docker.com/engine/reference/builder/#entrypoint). Specify this field as an array of executable and arguments, similar to a Docker <c>ENTRYPOINT</c>&apos;s &quot;exec&quot; form, not its &quot;shell&quot; form. If you do not specify this field, then the container&apos;s <c>ENTRYPOINT</c> runs, in conjunction with the args field or the container&apos;s [<c>CMD</c>](https://docs.docker.com/engine/reference/builder/#cmd), if either exists. If this field is not specified and the container does not have an <c>ENTRYPOINT</c>, then refer to the Docker documentation about [how <c>CMD</c> and <c>ENTRYPOINT</c> interact](https://docs.docker.com/engine/reference/builder/#understand-how-cmd-and-entrypoint-interact). If you specify this field, then you can also specify the <c>args</c> field to provide additional arguments for this command. However, if you specify this field, then the container&apos;s <c>CMD</c> is ignored. See the [Kubernetes documentation about how the <c>command</c> and <c>args</c> fields interact with a container&apos;s <c>ENTRYPOINT</c> and <c>CMD</c>](https://kubernetes.io/docs/tasks/inject-data-application/define-command-argument-container/#notes). In this field, you can reference [environment variables set by Vertex AI](https://cloud.google.com/vertex-ai/docs/predictions/custom-container-requirements#aip-variables) and environment variables set in the env field. You cannot reference environment variables set in the Docker image. In order for environment variables to be expanded, reference them by using the following syntax: $( VARIABLE_NAME) Note that this differs from Bash variable expansion, which does not use parentheses. If a variable cannot be resolved, the reference in the input string is used unchanged. To avoid variable expansion, you can escape this syntax with <c>$$</c>; for example: $$(VARIABLE_NAME) This field corresponds to the <c>command</c> field of the Kubernetes Containers [v1 core API](https://kubernetes.io/docs/reference/generated/kubernetes-api/v1.23/#container-v1-core).
		/// </summary>
		public List<string>? Command { get; set; }
		/// <summary>
		/// Immutable. Deployment timeout. Limit for deployment timeout is 2 hours.
		/// </summary>
		public string? DeploymentTimeout { get; set; }
		/// <summary>
		/// Immutable. List of environment variables to set in the container. After the container starts running, code running in the container can read these environment variables. Additionally, the command and args fields can reference these variables. Later entries in this list can also reference earlier entries. For example, the following example sets the variable <c>VAR_2</c> to have the value <c>foo bar</c>: ``<c>json [ { &quot;name&quot;: &quot;VAR_1&quot;, &quot;value&quot;: &quot;foo&quot; }, { &quot;name&quot;: &quot;VAR_2&quot;, &quot;value&quot;: &quot;$(VAR_1) bar&quot; } ] </c>`<c> If you switch the order of the variables in the example, then the expansion does not occur. This field corresponds to the </c>env` field of the Kubernetes Containers [v1 core API](https://kubernetes.io/docs/reference/generated/kubernetes-api/v1.23/#container-v1-core).
		/// </summary>
		public List<EnvVar>? Env { get; set; }
		/// <summary>
		/// Immutable. List of ports to expose from the container. Vertex AI sends gRPC prediction requests that it receives to the first port on this list. Vertex AI also sends liveness and health checks to this port. If you do not specify this field, gRPC requests to the container will be disabled. Vertex AI does not use ports other than the first one listed. This field corresponds to the <c>ports</c> field of the Kubernetes Containers v1 core API.
		/// </summary>
		public List<Port>? GrpcPorts { get; set; }
		/// <summary>
		/// Immutable. Specification for Kubernetes readiness probe.
		/// </summary>
		public Probe? HealthProbe { get; set; }
		/// <summary>
		/// Immutable. HTTP path on the container to send health checks to. Vertex AI intermittently sends GET requests to this path on the container&apos;s IP address and port to check that the container is healthy. Read more about [health checks](https://cloud.google.com/vertex-ai/docs/predictions/custom-container-requirements#health). For example, if you set this field to <c>/bar</c>, then Vertex AI intermittently sends a GET request to the <c>/bar</c> path on the port of your container specified by the first value of this <c>ModelContainerSpec</c>&apos;s ports field. If you don&apos;t specify this field, it defaults to the following value when you deploy this Model to an Endpoint: /v1/endpoints/ENDPOINT/deployedModels/ DEPLOYED_MODEL:predict The placeholders in this value are replaced as follows: * ENDPOINT: The last segment (following <c>endpoints/</c>)of the Endpoint.name][] field of the Endpoint where this Model has been deployed. (Vertex AI makes this value available to your container code as the [<c>AIP_ENDPOINT_ID</c> environment variable](https://cloud.google.com/vertex-ai/docs/predictions/custom-container-requirements#aip-variables).) * DEPLOYED_MODEL: DeployedModel.id of the <c>DeployedModel</c>. (Vertex AI makes this value available to your container code as the [<c>AIP_DEPLOYED_MODEL_ID</c> environment variable](https://cloud.google.com/vertex-ai/docs/predictions/custom-container-requirements#aip-variables).)
		/// </summary>
		public string? HealthRoute { get; set; }
		/// <summary>
		/// Required. Immutable. URI of the Docker image to be used as the custom container for serving predictions. This URI must identify an image in Artifact Registry or Container Registry. Learn more about the [container publishing requirements](https://cloud.google.com/vertex-ai/docs/predictions/custom-container-requirements#publishing), including permissions requirements for the Vertex AI Service Agent. The container image is ingested upon ModelService.UploadModel, stored internally, and this original path is afterwards not used. To learn about the requirements for the Docker image itself, see [Custom container requirements](https://cloud.google.com/vertex-ai/docs/predictions/custom-container-requirements#). You can use the URI to one of Vertex AI&apos;s [pre-built container images for prediction](https://cloud.google.com/vertex-ai/docs/predictions/pre-built-containers) in this field.
		/// </summary>
		public string? ImageUri { get; set; }
		/// <summary>
		/// Immutable. Invoke route prefix for the custom container. &quot;/*&quot; is the only supported value right now. By setting this field, any non-root route on this model will be accessible with invoke http call eg: &quot;/invoke/foo/bar&quot;, however the [PredictionService.Invoke] RPC is not supported yet. Only one of <c>predict_route</c> or <c>invoke_route_prefix</c> can be set, and we default to using <c>predict_route</c> if this field is not set. If this field is set, the Model can only be deployed to dedicated endpoint.
		/// </summary>
		public string? InvokeRoutePrefix { get; set; }
		/// <summary>
		/// Immutable. Specification for Kubernetes liveness probe.
		/// </summary>
		public Probe? LivenessProbe { get; set; }
		/// <summary>
		/// Immutable. List of ports to expose from the container. Vertex AI sends any prediction requests that it receives to the first port on this list. Vertex AI also sends [liveness and health checks](https://cloud.google.com/vertex-ai/docs/predictions/custom-container-requirements#liveness) to this port. If you do not specify this field, it defaults to following value: ``<c>json [ { &quot;containerPort&quot;: 8080 } ] </c>`<c> Vertex AI does not use ports other than the first one listed. This field corresponds to the </c>ports` field of the Kubernetes Containers [v1 core API](https://kubernetes.io/docs/reference/generated/kubernetes-api/v1.23/#container-v1-core).
		/// </summary>
		public List<Port>? Ports { get; set; }
		/// <summary>
		/// Immutable. HTTP path on the container to send prediction requests to. Vertex AI forwards requests sent using projects.locations.endpoints.predict to this path on the container&apos;s IP address and port. Vertex AI then returns the container&apos;s response in the API response. For example, if you set this field to <c>/foo</c>, then when Vertex AI receives a prediction request, it forwards the request body in a POST request to the <c>/foo</c> path on the port of your container specified by the first value of this <c>ModelContainerSpec</c>&apos;s ports field. If you don&apos;t specify this field, it defaults to the following value when you deploy this Model to an Endpoint: /v1/endpoints/ENDPOINT/deployedModels/DEPLOYED_MODEL:predict The placeholders in this value are replaced as follows: * ENDPOINT: The last segment (following <c>endpoints/</c>)of the Endpoint.name][] field of the Endpoint where this Model has been deployed. (Vertex AI makes this value available to your container code as the [<c>AIP_ENDPOINT_ID</c> environment variable](https://cloud.google.com/vertex-ai/docs/predictions/custom-container-requirements#aip-variables).) * DEPLOYED_MODEL: DeployedModel.id of the <c>DeployedModel</c>. (Vertex AI makes this value available to your container code as the [<c>AIP_DEPLOYED_MODEL_ID</c> environment variable](https://cloud.google.com/vertex-ai/docs/predictions/custom-container-requirements#aip-variables).)
		/// </summary>
		public string? PredictRoute { get; set; }
		/// <summary>
		/// Immutable. The amount of the VM memory to reserve as the shared memory for the model in megabytes.
		/// </summary>
		public long? SharedMemorySizeMb { get; set; }
		/// <summary>
		/// Immutable. Specification for Kubernetes startup probe.
		/// </summary>
		public Probe? StartupProbe { get; set; }
    }
}