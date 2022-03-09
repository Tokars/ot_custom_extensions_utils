using System;
using System.Text.RegularExpressions;

namespace OT.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Format strings: regex char: "_" to space.
        /// </summary>
        /// <param name="title">string_string</param>
        /// <returns>formated: string string</returns>
        public static string FormatEnumTitle(this string title)
        {
            string pattern = "_";
            string replacement = " ";

            Regex regEx = new Regex(pattern);
            return regEx.Replace(title, replacement);
        }

        public static string CamelCaseToSpace(this string input)
        {
            string strRegex = @"(?<=[a-z])([A-Z])|(?<=[A-Z])([A-Z][a-z])";
            Regex myRegex = new Regex(strRegex, RegexOptions.None);

            string strReplace = @" $1$2";

            return myRegex.Replace(input, strReplace);
        }

        public static string Replace(this string s, char[] separators, string newVal)
        {
            string[] temp;

            temp = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return String.Join(newVal, temp);
        }

        public static string ClearNewLineSpaces(this string s)
        {
            string replacement = Regex.Replace(s, @"\t|\n|\r", "");
            replacement = replacement.Replace(Environment.NewLine, String.Empty);
            return replacement;
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }
}