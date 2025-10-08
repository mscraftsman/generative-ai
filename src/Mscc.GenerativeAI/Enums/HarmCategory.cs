#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
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
        HarmCategoryHateSpeech,
        /// <summary>
        /// HarmCategoryDangerousContent means the harm category is dangerous content.
        /// </summary>
        HarmCategoryDangerousContent,
        /// <summary>
        /// HarmCategoryHarassment means the harm category is harassment.
        /// </summary>
        HarmCategoryHarassment,
        /// <summary>
        /// HarmCategorySexuallyExplicit means the harm category is sexually explicit content.
        /// </summary>
        HarmCategorySexuallyExplicit,
        /// <summary>
        /// Content that may be used to harm civic integrity.
        /// </summary>
        [Obsolete("Use EnableEnhancedCivicAnswers instead.")]
        HarmCategoryCivicIntegrity,
        /// <summary>
        /// The harm category is for jailbreak prompts.
        /// </summary>
        HarmCategoryJailbreak,
        /// <summary>
        /// The harm category is image hate.
        /// </summary>
        HarmCategoryImageHate,
        /// <summary>
        /// The harm category is image dangerous content.
        /// </summary>
        HarmCategoryImageDangerousContent,
        /// <summary>
        /// The harm category is image harassment.
        /// </summary>
        HarmCategoryImageHarassment,
        /// <summary>
        /// The harm category is image sexually explicit content.
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
