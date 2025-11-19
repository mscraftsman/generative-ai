using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<Category>))]
    public enum Category
    {
        /// <summary>
        /// Category is unspecified.
        /// </summary>
        HarmCategoryUnspecified,
        /// <summary>
        /// **PaLM** - Negative or harmful comments targeting identity and/or protected attribute.
        /// </summary>
        HarmCategoryDerogatory,
        /// <summary>
        /// **PaLM** - Content that is rude, disrespectful, or profane.
        /// </summary>
        HarmCategoryToxicity,
        /// <summary>
        /// **PaLM** - Describes scenarios depicting violence against an individual or group, or general descriptions of gore.
        /// </summary>
        HarmCategoryViolence,
        /// <summary>
        /// **PaLM** - Contains references to sexual acts or other lewd content.
        /// </summary>
        HarmCategorySexual,
        /// <summary>
        /// **PaLM** - Promotes unchecked medical advice.
        /// </summary>
        HarmCategoryMedical,
        /// <summary>
        /// **PaLM** - Dangerous content that promotes, facilitates, or encourages harmful acts.
        /// </summary>
        HarmCategoryDangerous,
        /// <summary>
        /// **Gemini** - Harassment content.
        /// </summary>
        HarmCategoryHarassment,
        /// <summary>
        /// **Gemini** - Hate speech and content.
        /// </summary>
        HarmCategoryHateSpeech,
        /// <summary>
        /// **Gemini** - Sexually explicit content.
        /// </summary>
        HarmCategorySexuallyExplicit,
        /// <summary>
        /// **Gemini** - Dangerous content.
        /// </summary>
        HarmCategoryDangerousContent,
        /// <summary>
        /// **Gemini** - Content that may be used to harm civic integrity. DEPRECATED: use enable_enhanced_civic_answers instead.
        /// </summary>
        HarmCategoryCivicIntegrity,
    }
}