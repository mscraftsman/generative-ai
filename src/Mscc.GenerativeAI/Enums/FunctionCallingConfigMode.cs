#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Mode of function calling to define the execution behavior for function calling.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<FunctionCallingConfigMode>))]
    public enum FunctionCallingConfigMode
    {
        /// <summary>
        /// Unspecified function calling mode. This value should not be used.
        /// </summary>
        ModeUnspecified = 0,
        /// <summary>
        /// Default model behavior, model decides to predict either a function call
        /// or a natural language response.
        /// </summary>
        Auto,
        /// <summary>
        /// Model is constrained to always predicting a function call only.
        /// If "allowed_function_names" are set, the predicted function call will be
        /// limited to any one of "allowed_function_names", else the predicted
        /// function call will be any one of the provided "function_declarations".
        /// </summary>
        Any,
        /// <summary>
        /// Model will not predict any function call. Model behavior is same as when
        /// not passing any function declarations.
        /// </summary>
        None,
        /// <summary>
        /// Model decides to predict either a function call or a natural language response,
        /// but will validate function calls with constrained decoding.
        /// </summary>
        Validated
    }
}