﻿using System;
using System.Text.RegularExpressions;

namespace baggybot_stats.Tools
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

		private static string ToCamelOrPascalCase(string str, Func<char, char> firstLetterTransform)
		{
			var input = str;
			var pattern = "([_\\-])(?<char>[a-z])";
			var num = 1;
			var str1 = Regex.Replace(input, pattern, (MatchEvaluator)(match => match.Groups["char"].Value.ToUpperInvariant()), (RegexOptions)num);
			return firstLetterTransform(str1[0]).ToString() + str1.Substring(1);
		}

		/// <summary>
		/// Convert the string with underscores (this_is_a_test) or hyphens (this-is-a-test) to
		/// camel case (thisIsATest). Camel case is the same as Pascal case, except the first letter
		/// is lowercase.
		/// </summary>
		/// <param name="str">String to convert</param>
		/// <returns>
		/// Converted string
		/// </returns>
		public static string ToCamelCase(this string str)
		{
			return ToCamelOrPascalCase(str, new Func<char, char>(char.ToLowerInvariant));
		}

		/// <summary>
		/// Convert the string with underscores (this_is_a_test) or hyphens (this-is-a-test) to
		/// pascal case (ThisIsATest). Pascal case is the same as camel case, except the first letter
		/// is uppercase.
		/// </summary>
		/// <param name="str">String to convert</param>
		/// <returns>
		/// Converted string
		/// </returns>
		public static string ToPascalCase(this string str)
		{
			return ToCamelOrPascalCase(str, new Func<char, char>(char.ToUpperInvariant));
		}

		/// <summary>
		/// Convert the string from camelcase (thisIsATest) to a hyphenated (this-is-a-test) or
		/// underscored (this_is_a_test) string
		/// </summary>
		/// <param name="str">String to convert</param><param name="separator">Separator to use between segments</param>
		/// <returns>
		/// Converted string
		/// </returns>
		public static string FromCamelCase(this string str, string separator)
		{
			str = char.ToLower(str[0]).ToString() + str.Substring(1);
			str = Regex.Replace(ToCamelCase(str), "(?<char>[A-Z])", (MatchEvaluator)(match => separator + match.Groups["char"].Value.ToLowerInvariant()));
			return str;
		}
	}
}
