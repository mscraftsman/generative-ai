namespace Mscc.GenerativeAI
{
    /// <summary>
    /// An input/output example used to instruct the Model.
    /// It demonstrates how the model should respond or format its response.
    /// </summary>
    public class Example
    {
        /// <summary>
        /// Required. An example of an input Message from the user.
        /// </summary>
        public Message Input { get; set; }
        /// <summary>
        /// Required. An example of what the model should output given the input.
        /// </summary>
        public Message Output { get; set; }
    }
}