using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    [JsonConverter(typeof(JsonStringEnumConverter<HarmCategory>))]
    public enum HarmCategory
    {
        /// <summary>
        /// HarmCategoryUnspecified means the harm category is unspecified.
        /// </summary>
        HarmCategoryUnspecified = 0,
        /// <summary>
        /// HarmCategoryHateSpeech means the harm category is hate speech.
        /// </summary>
        HarmCategoryHateSpeech = 1,
        /// <summary>
        /// HarmCategoryDangerousContent means the harm category is dangerous content.
        /// </summary>
        HarmCategoryDangerousContent = 2,
        /// <summary>
        /// HarmCategoryHarassment means the harm category is harassment.
        /// </summary>
        HarmCategoryHarassment = 3,
        /// <summary>
        /// HarmCategorySexuallyExplicit means the harm category is sexually explicit content.
        /// </summary>
        HarmCategorySexuallyExplicit = 4
    }
}
