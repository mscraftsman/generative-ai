namespace Mscc.GenerativeAI
{
    /// <summary>
    /// This is a copy of google.longrunning.Operation.
    /// We need to copy it because for interacting with scotty, we need to add a scotty specific field that can't be added in the top level Operation proto.
    /// </summary>
    public class CustomLongRunningOperation : IOperation
    {
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public bool Done { get; set; }
        /// <inheritdoc/>
        public Status? Error { get; set; }
        /// <inheritdoc/>
        public object? Metadata { get; set; }
        /// <inheritdoc/>
        public object? Response { get; set; }
    }
}