using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Mscc.GenerativeAI
{
    internal static class ConverterExtensions
    {
 /// <summary>
        /// Converts a given string to snake case.
        /// </summary>
        /// <param name="text">The string to be converted to snake case.</param>
        /// <returns>The resulting snake case string.</returns>
        public static string ToSnakeCase(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            // Create a new instance of StringBuilder to store the output string with an estimated capacity
            // Nullable UnicodeCategory variable to keep track of the previous category
            StringBuilder builder = new(text.Length + Math.Min(2, text.Length / 5));
            UnicodeCategory? previousCategory = default;

            // Iterate over each character in the input string
            for (int currentIndex = 0; currentIndex < text.Length; currentIndex++)
            {
                // Get the current character
                char currentChar = text[currentIndex];

                // If the current character is already an underscore, append it to the output string
                if (currentChar == '_')
                {
                    builder.Append('_');
                    previousCategory = null;
                    continue;
                }

                // Get the Unicode category of the current character
                UnicodeCategory currentCategory = char.GetUnicodeCategory(currentChar);

                switch (currentCategory)
                {
                    // If the current character is an uppercase letter or titlecase letter
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.TitlecaseLetter:
                        // If the previous character is a space, lowercase letter or decimal digit,
                        // and the next character is a lowercase letter, append an underscore to the output string
                        if (previousCategory == UnicodeCategory.SpaceSeparator ||
                            previousCategory == UnicodeCategory.LowercaseLetter ||
                            previousCategory != UnicodeCategory.DecimalDigitNumber &&
                            previousCategory != null &&
                            currentIndex > 0 &&
                            currentIndex + 1 < text.Length &&
                            char.IsLower(text[currentIndex + 1]))
                        {
                            builder.Append('_');
                        }

                        // Convert the current character to lowercase
                        currentChar = char.ToLower(currentChar, CultureInfo.InvariantCulture);
                        break;

                    // If the current character is a lowercase letter or decimal digit
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        // If the previous character is a space, append an underscore to the output string
                        if (previousCategory == UnicodeCategory.SpaceSeparator)
                        {
                            builder.Append('_');
                        }
                        break;

                    // If the current character is a separator, punctuation mark or symbol
                    default:
                        // If the previous category is not null, set it to a space separator
                        if (previousCategory != null)
                        {
                            previousCategory = UnicodeCategory.SpaceSeparator;
                        }
                        continue;
                }

                // Append the current character to the output string
                builder.Append(currentChar);

                // Update the previous category to the current category
                previousCategory = currentCategory;
            }

            // Return the resulting snake case string
            return builder.ToString();
        }


        /// <summary>
        /// Converts a given string to camel case.
        /// </summary>
        /// <param name="text">The string to be converted to camel case.</param>
        /// <param name="removeWhitespace">Whether to remove whitespace or not.</param>
        /// <param name="preserveLeadingUnderscore">Whether to preserve the leading underscore or not.</param>
        /// <returns>The resulting camel case string.</returns>
        public static string ToCamelCase(this string text,
                                         bool removeWhitespace = true,
                                         bool preserveLeadingUnderscore = false)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text; // if text is null or empty, return it as it is.
            }

            if (text.IsAllUpper())
            {
                text = text.ToLower(); // if the text is all uppercase, convert it to lowercase
            }

            // Check if the leading underscore should be preserved
            bool addLeadingUnderscore = preserveLeadingUnderscore && text.StartsWith("_");

            // Create a new instance of StringBuilder to store the output string
            StringBuilder result = new(text.Length);
            
            // Flag to keep track of whether the current character should be uppercase or not
            bool toUpper = false;

            // Iterate over each character in the input string
            foreach (char c in text)
            {
                // If the current character is a separator or whitespace and the whitespace is to be removed, set the flag to true
                if (c == '-' || c == '_' || (removeWhitespace && char.IsWhiteSpace(c)))
                {
                    toUpper = true;
                }
                else
                {
                    // Append the current character to the output string in uppercase or lowercase based on the flag, and reset the flag to false
                    result.Append(toUpper ? char.ToUpperInvariant(c) : c);
                    toUpper = false;
                }
            }

            if (result.Length > 0)
            {
                // Convert the first character to lowercase
                result[0] = char.ToLowerInvariant(result[0]);
            }

            if (addLeadingUnderscore)
            {
                // Insert the leading underscore at the beginning of the string
                result.Insert(0, '_');
            }

            // Return the resulting camel case string
            return result.ToString();
        }

        /// <summary>
        /// Converts the specified string to PascalCase.
        /// </summary>
        /// <param name="text">The string to convert.</param>
        /// <returns>The PascalCase version of the string.</returns>
        public static string ToPascalCase(this string text)
        {
            // Create a StringBuilder object to store the result.
            StringBuilder result = new();

            // Get the TextInfo object for the current culture.
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

            // Flag to track if we are at the beginning of a new word.
            bool newWord = true;

            // Iterate over each character in the string.
            for (int i = 0; i < text.Length; i++)
            {
                char currentChar = text[i];

                // If the current character is a letter or digit.
                if (char.IsLetterOrDigit(currentChar))
                {
                    // If we are at the beginning of a new word, convert the character to uppercase.
                    if (newWord)
                    {
                        result.Append(textInfo.ToUpper(currentChar));
                        newWord = false;
                    }
                    // Otherwise, add the character as is for uppercase or convert to lowercase for other characters.
                    else
                    {
                        result.Append(i < text.Length - 1 && char.IsUpper(currentChar) && char.IsLower(text[i + 1]) ? currentChar : char.ToLowerInvariant(currentChar));
                    }
                }
                // If the current character is not a letter or digit, we are at the beginning of a new word.
                else
                {
                    newWord = true;
                }

                // If the current character is a lowercase letter and the next character is an uppercase letter,
                // we are at the beginning of a new word.
                if (i < text.Length - 1 && char.IsLower(text[i]) && char.IsUpper(text[i + 1]))
                {
                    newWord = true;
                }
            }

            // Return the result as a string.
            return result.ToString();
        }

        /// <summary>
        /// Splits a given camel case string into separate words using the specified separator.
        /// </summary>
        /// <param name="input">The camel case string to be split.</param>
        /// <param name="splitWith">The separator to be used. By default, a single space is used.</param>
        /// <returns>The resulting string with words separated by the specified separator.</returns>
        public static string SplitCamelCase(this string input, string splitWith = " ")
        {
            if (string.IsNullOrEmpty(input)) return input; // if input is null or empty, return it as it is.

            // Create a new instance of StringBuilder to store the output string
            StringBuilder result = new();
            // Flag to keep track of whether the previous character was an uppercase letter or not
            bool isPrevUpper = false;

            // Iterate over each character in the input string
            for (int i = 0; i < input.Length; i++)
            {
                // Get the current character
                char currentChar = input[i];

                // If the current character is uppercase and not the first character
                if (i > 0 && char.IsUpper(currentChar))
                {
                    // If the previous character was not uppercase or the next character is not uppercase
                    if (!isPrevUpper || (i < input.Length - 1 && !char.IsUpper(input[i + 1])))
                    {
                        // Append the separator to the output string
                        result.Append(splitWith);
                    }
                }

                // Append the current character to the output string
                result.Append(currentChar);
                // Update the flag to reflect whether the current character is uppercase or not
                isPrevUpper = char.IsUpper(currentChar);
            }

            // Return the resulting string with words separated by the specified separator
            return result.ToString();
        }
        
        /// <summary>
        /// Converts a string to kebab-case, with words separated by hyphens.
        /// </summary>
        /// <param name="text">The input string to be converted to kebab-case.</param>
        /// <returns>A kebab-case representation of the input string.</returns>
        public static string ToKebabCase(this string text)
        {
            // Return the input text if it's null or empty
            if (string.IsNullOrEmpty(text)) return text;

            // Initialize a StringBuilder to store the result
            StringBuilder result = new();
            // Define a flag to track whether the previous character is a separator
            bool previousCharacterIsSeparator = true;

            // Iterate through each character in the input text
            for (int i = 0; i < text.Length; i++)
            {
                char currentChar = text[i];

                // If the current character is an uppercase letter or a digit
                if (char.IsUpper(currentChar) || char.IsDigit(currentChar))
                {
                    // Add a hyphen if the previous character is not a separator and
                    // the current character is preceded by a lowercase letter or followed by a lowercase letter
                    if (!previousCharacterIsSeparator && (i > 0 && (char.IsLower(text[i - 1]) || (i < text.Length - 1 && char.IsLower(text[i + 1])))))
                    {
                        result.Append("-");
                    }

                    // Append the lowercase version of the current character to the result
                    result.Append(char.ToLowerInvariant(currentChar));
                    // Update the flag to indicate that the current character is not a separator
                    previousCharacterIsSeparator = false;
                }
                // If the current character is a lowercase letter
                else if (char.IsLower(currentChar))
                {
                    // Append the current character to the result
                    result.Append(currentChar);
                    // Update the flag to indicate that the current character is not a separator
                    previousCharacterIsSeparator = false;
                }
                // If the current character is a space, underscore, or hyphen
                else if (currentChar == ' ' || currentChar == '_' || currentChar == '-')
                {
                    // Add a hyphen if the previous character is not a separator
                    if (!previousCharacterIsSeparator)
                    {
                        result.Append("-");
                    }
                    // Update the flag to indicate that the current character is a separator
                    previousCharacterIsSeparator = true;
                }
            }

            // Return the kebab-case representation of the input string
            return result.ToString();
        }

#if NETSTANDARD2_0 || NETSTANDARD2_1 || NETCOREAPP3_1 || NET5_0

        /// <summary>
        /// Extension method to convert a given string to title case.
        /// </summary>
        /// <param name="text">The string to convert to title case.</param>
        /// <returns>A new string with each word in title case.</returns>
        public static string ToTitleCase(this string text)
        {
            // Check if the input string is null or empty, return the original string in that case
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            // Create a StringBuilder with the initial capacity set to the length of the input string
            // This helps avoid unnecessary memory allocations
            StringBuilder sb = new(text.Length);

            // A boolean flag to track if we are at the start of a new word
            bool newWord = true;

            // Iterate over each character in the input string
            foreach (char c in text)
            {
                // If the current character is a whitespace, hyphen, or underscore,
                // set the newWord flag to true and append the character to the StringBuilder
                if (char.IsWhiteSpace(c) || c is '-' or '_')
                {
                    newWord = true;
                    sb.Append(c);
                }
                // If we are at the start of a new word, append the uppercase version of the character
                // and set the newWord flag to false
                else if (newWord)
                {
                    sb.Append(char.ToUpper(c));
                    newWord = false;
                }
                // If we are not at the start of a new word, append the lowercase version of the character
                else
                {
                    sb.Append(char.ToLower(c));
                }
            }

            // Convert the StringBuilder to a string and return it
            return sb.ToString();
        }

#elif NET6_0_OR_GREATER

        /// <summary>
        /// Extension method to convert a given string to title case.
        /// </summary>
        /// <param name="text">The string to convert to title case.</param>
        /// <returns>A new string with each word in title case.</returns>
        public static string ToTitleCase(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            // Use a StringBuilder to store the output string
            StringBuilder builder = new(text.Length * 2);
            bool newWord = true;

            foreach (Rune rune in text.EnumerateRunes())
            {
                if (Rune.IsWhiteSpace(rune) || rune.Value is '-' or '_')
                {
                    newWord = true;
                    builder.Append(rune);
                }
                else if (newWord)
                {
                    builder.Append(Rune.ToUpper(rune, CultureInfo.InvariantCulture));
                    newWord = false;
                }
                else
                {
                    builder.Append(Rune.ToLower(rune, CultureInfo.InvariantCulture));
                }
            }

            return builder.ToString();
        }


#endif

        /// <summary>
        /// Convert a string to Train Case
        /// </summary>
        /// <param name="text"></param>
        /// <returns>string</returns>
        public static string ToTrainCase(this string text)
        {
            return text.ToPascalCase().SplitCamelCase("-").FirstCharToUpperCase().Replace("--", "-");
        }

        /// <summary>
        /// Insert any character before all upper space characters in a string
        /// </summary>
        /// <param name="text"></param>
        /// <param name="character"></param>
        /// <returns>string</returns>
        public static string InsertCharacterBeforeUpperCase(this string text,
                                                            char character = ' ')
        {
            StringBuilder sb = new();
            char previousChar = char.MinValue; // Unicode '\0'
            foreach (char c in text)
            {
                if (char.IsUpper(c))
                {
                    // If not the first character and previous character is not a space, insert character before uppercase
                    if (sb.Length != 0 && previousChar != ' ')
                    {
                        sb.Append(character);
                    }
                }
                sb.Append(c);
                previousChar = c;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Insert a space before any upper case character in a string
        /// </summary>
        /// <param name="text"></param>
        /// <returns>string</returns>
        public static string InsertSpaceBeforeUpperCase(this string text)
        {
            return text.InsertCharacterBeforeUpperCase();
        }

        /// <summary>
        /// Replace specific characters found in a string
        /// See: https://stackoverflow.com/a/7265786/7986443
        /// </summary>
        /// <param name="s"></param>
        /// <param name="separators"></param>
        /// <param name="newVal"></param>
        /// <returns>string</returns>
        public static string Replace(this string s, char[] separators, string newVal)
        {
            string[] temp = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(newVal, temp);
        }

        private static readonly Regex WhiteSpaceRegex = new(@"\s+");

        /// <summary>
        /// Replace all whitespace in a string
        /// See: https://stackoverflow.com/questions/6219454/efficient-way-to-remove-all-whitespace-from-string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="replacement"></param>
        /// <returns>string</returns>
        public static string ReplaceWhitespace(this string input, string replacement)
        {
            return WhiteSpaceRegex.Replace(input, replacement);
        }


        /// <summary>
        /// Extension method to check if all the letters in the input string are uppercase.
        /// </summary>
        /// <param name="input">The string to check for uppercase letters.</param>
        /// <returns>True if all the letters in the input string are uppercase, otherwise false.</returns>
        public static bool IsAllUpper(this string input)
        {
            // Return early if the input string is null or empty
            if (string.IsNullOrEmpty(input))
            {
                return true;
            }

            // Iterate over each character in the input string
            foreach (char c in input)
            {
                // If the current character is a letter and not uppercase, return false
                if (char.IsLetter(c) && !char.IsUpper(c))
                {
                    return false;
                }
            }

            // If all characters are either uppercase letters or non-letter characters, return true
            return true;
        }

        /// <summary>
        /// Convert SnakeCase to CamelCase
        /// See: https://www.codegrepper.com/code-examples/csharp/camelCase+and+snakeCase
        /// </summary>
        /// <param name="text"></param>
        /// <returns>string</returns>
        public static string SnakeCaseToCamelCase(this string text)
        {
            return Regex.Replace(text, "_[a-z]", m => m.ToString().TrimStart('_').ToUpper()).FirstCharToLowerCase();
        }

        /// <summary>
        /// Convert the first character in a string to lower case
        /// See: https://stackoverflow.com/questions/21755757/first-character-of-string-lowercase-c-sharp/21755933
        /// </summary>
        /// <param name="text"></param>
        /// <returns>string</returns>
        public static string FirstCharToLowerCase(this string text)
        {
            if (string.IsNullOrEmpty(text) || char.IsLower(text[0]))
                return text;

            return char.ToLower(text[0]) + text.Substring(1);
        }

        /// <summary>
        /// Convert the first character in a string to upper case
        /// </summary>
        /// <param name="text"></param>
        /// <returns>string</returns>
        public static string FirstCharToUpperCase(this string text)
        {
            if (string.IsNullOrEmpty(text) || char.IsUpper(text[0]))
                return text;

            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}