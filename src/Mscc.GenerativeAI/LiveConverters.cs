using System.Text.Json.Nodes;

namespace Mscc.GenerativeAI
{
    internal class LiveConverters
    {
        public JsonNode LiveConnectParametersToVertex(JsonNode parameterNode)
        {
            if (parameterNode is not JsonObject obj) return parameterNode;

            var result = new JsonObject();
            if (obj.TryGetPropertyValue("model", out var model)) result["model"] = model.DeepClone();
            if (obj.TryGetPropertyValue("config", out var config))
            {
                if (config is JsonObject configObj)
                {
                    foreach (var prop in configObj)
                    {
                        result[prop.Key] = prop.Value?.DeepClone();
                    }
                }
            }

            return result;
        }

        public JsonNode LiveConnectParametersToMldev(JsonNode parameterNode)
        {
            return LiveConnectParametersToVertex(parameterNode);
        }

        public JsonNode LiveClientMessageToVertex(JsonNode messageNode)
        {
            return messageNode;
        }

        public JsonNode LiveClientMessageToMldev(JsonNode messageNode)
        {
            return messageNode;
        }
    }
}
