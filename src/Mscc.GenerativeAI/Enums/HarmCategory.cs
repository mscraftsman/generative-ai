using System;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The harm category to be blocked.
    /// Ref: https://ai.google.dev/api/rest/v1beta/HarmCategory
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<HarmCategory>))]
    public enum HarmCategory
    {
        /// <summary>
        /// Default value. This value is unused.
        /// </summary>
        HarmCategoryUnspecified = 0,
        /// <summary>
        /// Content that promotes violence or incites hatred against individuals or groups based on
        /// </summary>
        HarmCategoryHateSpeech,
        /// <summary>
        /// Content that promotes, facilitates, or enables dangerous activities.
        /// </summary>
        HarmCategoryDangerousContent,
        /// <summary>
        /// Abusive, threatening, or content intended to bully, torment, or ridicule.
        /// </summary>
        HarmCategoryHarassment,
        /// <summary>
        /// Content that contains sexually explicit material.
        /// </summary>
        HarmCategorySexuallyExplicit,
        /// <summary>
        /// Deprecated: Election filter is not longer supported. The harm category is civic integrity.
        /// </summary>
        [Obsolete("Use EnableEnhancedCivicAnswers instead.")]
        HarmCategoryCivicIntegrity,
        /// <summary>
        /// Prompts designed to bypass safety filters. This enum value is not supported in Gemini API.
        /// </summary>
        HarmCategoryJailbreak,
        /// <summary>
        /// Images that contain hate speech. This enum value is not supported in Gemini API.
        /// </summary>
        HarmCategoryImageHate,
        /// <summary>
        /// Images that contain dangerous content. This enum value is not supported in Gemini API.
        /// </summary>
        HarmCategoryImageDangerousContent,
        /// <summary>
        /// Images that contain harassment. This enum value is not supported in Gemini API.
        /// </summary>
        HarmCategoryImageHarassment,
        /// <summary>
        /// Images that contain sexually explicit content. This enum value is not supported in Gemini
        /// </summary>
        HarmCategoryImageSexuallyExplicit,
        
        #region "PaLM 2" safety settings
        
        /// <summary>
        /// Negative or harmful comments targeting identity and/or protected attribute.
        /// </summary>
        [Obsolete("This value is related to PaLM 2 models and features.")]
        HarmCategoryDerogatory = 101,
        /// <summary>
        /// Content that is rude, disrespectful, or profane.
        /// </summary>
        [Obsolete("This value is related to PaLM 2 models and features.")]
        HarmCategoryToxicity,
        /// <summary>
        /// Describes scenarios depicting violence against an individual or group, or general descriptions of gore.
        /// </summary>
        [Obsolete("This value is related to PaLM 2 models and features.")]
        HarmCategoryViolence,
        /// <summary>
        /// Contains references to sexual acts or other lewd content.
        /// </summary>
        [Obsolete("This value is related to PaLM 2 models and features.")]
        HarmCategorySexual,
        /// <summary>
        /// Promotes unchecked medical advice.
        /// </summary>
        [Obsolete("This value is related to PaLM 2 models and features.")]
        HarmCategoryMedical,
        /// <summary>
        /// Dangerous content that promotes, facilitates, or encourages harmful acts.
        /// </summary>
        [Obsolete("This value is related to PaLM 2 models and features.")]
        HarmCategoryDangerous
        
        #endregion
    }
}
