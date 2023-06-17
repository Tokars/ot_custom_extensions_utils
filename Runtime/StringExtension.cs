using System;
using System.Text.RegularExpressions;

namespace OT.Extensions
{
    public static class StringExtension
    {
        public static string ToCamelCase(this string s)
        {
            var x = s.Replace("_", "");
            if (x.Length == 0) return "null";
            x = Regex.Replace(x, "([A-Z])([A-Z]+)($|[A-Z])",
                m => m.Groups[1].Value + m.Groups[2].Value.ToLower() + m.Groups[3].Value);
            return char.ToLower(x[0]) + x.Substring(1);
        }

        public static string ToPascalCase(this string s)
        {
            var x = ToCamelCase(s);
            return char.ToUpper(x[0]) + x.Substring(1);
        }
        
        public static string SnakeCaseToSpace(this string title)
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