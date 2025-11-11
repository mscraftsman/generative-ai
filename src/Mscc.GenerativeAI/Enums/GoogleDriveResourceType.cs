using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    [JsonConverter(typeof(JsonStringEnumConverter<GoogleDriveResourceType>))]
    public enum GoogleDriveResourceType
    {
        ResourceTypeUnspecified = 0,
        ResourceTypeFolder,
        ResourceTypeFile
    }
}