using System.Text.RegularExpressions;

namespace Shared.Extensions
{
    public static class StringExtensions {

        /// <summary>
        /// Splits a string by capital letters.
        /// </summary>
        /// <example>
        /// "HelloWorld" -> ["Hello", "World"]
        /// </example>
        public static string[] SplitCamelCase(string source) {
            return Regex.Split(source, @"(?<!^)(?=[A-Z])");
        }

        /// <summary>
        /// Converts a camel case string to a sentence.
        /// </summary>
        /// <example>
        /// "HelloWorld" -> "Hello World"
        /// </example>
        public static string CamelCaseToSentence(this string source) {
            return string.Join(" ", SplitCamelCase(source));
        }
    }
}
