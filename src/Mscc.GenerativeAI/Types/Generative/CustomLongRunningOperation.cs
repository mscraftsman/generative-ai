namespace Mscc.GenerativeAI
{
    /// <summary>
    /// This is a copy of google.longrunning.Operation.
    /// We need to copy it because for interacting with scotty, we need to add a scotty specific field that can't be added in the top level Operation proto.
    /// </summary>
    public class CustomLongRunningOperation : Operation
    {
        
    }
}