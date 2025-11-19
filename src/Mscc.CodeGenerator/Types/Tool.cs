namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Tool details that the model may use to generate response. A <see cref="Tool"/> is a piece of code that enables the system to interact with external systems to perform an action, or set of actions, outside of knowledge and scope of the model. Next ID: 13
	/// </summary>
	public partial class Tool
	{
		/// <summary>
		/// Optional. Enables the model to execute code as part of generation.
		/// </summary>
		public CodeExecution? CodeExecution { get; set; }
		/// <summary>
		/// Optional. Tool to support the model interacting directly with the computer. If enabled, it automatically populates computer-use specific Function Declarations.
		/// </summary>
		public ComputerUse? ComputerUse { get; set; }
		/// <summary>
		/// Optional. FileSearch tool type. Tool to retrieve knowledge from Semantic Retrieval corpora.
		/// </summary>
		public FileSearch? FileSearch { get; set; }
		/// <summary>
		/// Optional. A list of <see cref="FunctionDeclarations"/> available to the model that can be used for function calling. The model or system does not execute the function. Instead the defined function may be returned as a FunctionCall with arguments to the client side for execution. The model may decide to call a subset of these functions by populating FunctionCall in the response. The next conversation turn may contain a FunctionResponse with the Content.role "function" generation context for the next model turn.
		/// </summary>
		public List<FunctionDeclaration>? FunctionDeclarations { get; set; }
		/// <summary>
		/// Optional. Tool that allows grounding the model's response with geospatial context related to the user's query.
		/// </summary>
		public GoogleMaps? GoogleMaps { get; set; }
		/// <summary>
		/// Optional. GoogleSearch tool type. Tool to support Google Search in Model. Powered by Google.
		/// </summary>
		public GoogleSearch? GoogleSearch { get; set; }
		/// <summary>
		/// Optional. Retrieval tool that is powered by Google search.
		/// </summary>
		public GoogleSearchRetrieval? GoogleSearchRetrieval { get; set; }
		/// <summary>
		/// Optional. Tool to support URL context retrieval.
		/// </summary>
		public UrlContext? UrlContext { get; set; }
    }
}
