#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The category of a rating.
    /// Ref: https://ai.google.dev/api/rest/v1beta/HarmCategory
    /// </summary>
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
        HarmCategorySexuallyExplicit = 4,
        
        /// <summary>
        /// Negative or harmful comments targeting identity and/or protected attribute.
        /// </summary>
        HarmCategoryDerogatory = 101,
        /// <summary>
        /// Content that is rude, disrespectful, or profane.
        /// </summary>
        HarmCategoryToxicity,
        /// <summary>
        /// Describes scenarios depicting violence against an individual or group, or general descriptions of gore.
        /// </summary>
        HarmCategoryViolence,
        /// <summary>
        /// Contains references to sexual acts or other lewd content.
        /// </summary>
        HarmCategorySexual,
        /// <summary>
        /// Promotes unchecked medical advice.
        /// </summary>
        HarmCategoryMedical,
        /// <summary>
        /// Dangerous content that promotes, facilitates, or encourages harmful acts.
        /// </summary>
        HarmCategoryDangerous
    }
}
