using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using OneClickSolutions.Infrastructure.Collections;

namespace OneClickSolutions.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex MatchArabicHebrew = new Regex(@"[\u0600-\u06FF,\u0590-\u05FF]",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Indicates whether the input text contains rtl data or not
        /// </summary>
        /// <param name="data">input text</param>
        /// <returns></returns>
        public static bool ContainsRtlText(this string data)
        {
            if (string.IsNullOrEmpty(data)) return false;
            return MatchArabicHebrew.IsMatch(data);
        }

        public static string PackToString(this IEnumerable<string> values, string packingSymbol)
        {
            return string.Join(packingSymbol, values);
        }
        
        public static IEnumerable<string> UnpackFromString(this string packedValues,string packingSymbol)
        {
            if (packedValues == null) throw new ArgumentNullException(nameof(packedValues));

            return packedValues.Split(new[] {packingSymbol}, StringSplitOptions.None);
        }

        /// <summary>
        /// Adds a char to end of given string if it does not ends with the char.
        /// </summary>
        public static string EnsureEndsWith(this string text, char c, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (text.EndsWith(c.ToString(), comparisonType))
            {
                return text;
            }

            return text + c;
        }

        /// <summary>
        /// Adds a char to end of given string if it does not ends with the char.
        /// </summary>
        public static string EnsureEndsWith(this string text, char symbol, bool ignoreCase, CultureInfo culture)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (text.EndsWith(symbol.ToString(culture), ignoreCase, culture))
            {
                return text;
            }

            return text + symbol;
        }

        /// <summary>
        /// Adds a char to beginning of given string if it does not starts with the char.
        /// </summary>
        public static string EnsureStartsWith(this string text, char symbol, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (text.StartsWith(symbol.ToString(), comparisonType))
            {
                return text;
            }

            return symbol + text;
        }

        /// <summary>
        /// Adds a char to beginning of given string if it does not starts with the char.
        /// </summary>
        public static string EnsureStartsWith(this string text, char symbol, bool ignoreCase, CultureInfo culture)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (text.StartsWith(symbol.ToString(culture), ignoreCase, culture))
            {
                return text;
            }

            return symbol + text;
        }

        /// <summary>
        /// Indicates whether this string is null or an System.String.Empty string.
        /// </summary>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// indicates whether this string is null, empty, or consists only of white-space characters.
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="text"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
        public static string Left(this string text, int len)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (text.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return text.Substring(0, len);
        }

        /// <summary>
        /// Converts line endings in the string to <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string NormalizeLineEndings(this string text)
        {
            return text.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
        }

        /// <summary>
        /// Gets index of nth occurence of a char in a string.
        /// </summary>
        /// <param name="str">source string to be searched</param>
        /// <param name="c">Char to search in <see cref="str"/></param>
        /// <param name="n">Count of the occurence</param>
        public static int NthIndexOf(this string str, char c, int n)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            var count = 0;
            for (var i = 0; i < str.Length; i++)
            {
                if (str[i] != c)
                {
                    continue;
                }

                if ((++count) == n)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Removes first occurrence of the given postfixes from end of the given string.
        /// Ordering is important. If one of the postFixes is matched, others will not be tested.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="postFixes">one or more postfix.</param>
        /// <returns>Modified string or the same string if it has not any of given postfixes</returns>
        public static string RemovePostFix(this string str, params string[] postFixes)
        {
            if (str == null)
            {
                return null;
            }

            if (str == string.Empty)
            {
                return string.Empty;
            }

            if (postFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var postFix in postFixes)
            {
                if (str.EndsWith(postFix))
                {
                    return str.Left(str.Length - postFix.Length);
                }
            }

            return str;
        }

        /// <summary>
        /// Removes first occurrence of the given prefixes from beginning of the given string.
        /// Ordering is important. If one of the preFixes is matched, others will not be tested.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="preFixes">one or more prefix.</param>
        /// <returns>Modified string or the same string if it has not any of given prefixes</returns>
        public static string RemovePreFix(this string str, params string[] preFixes)
        {
            if (str == null)
            {
                return null;
            }

            if (str == string.Empty)
            {
                return string.Empty;
            }

            if (preFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var preFix in preFixes)
            {
                if (str.StartsWith(preFix))
                {
                    return str.Right(str.Length - preFix.Length);
                }
            }

            return str;
        }

        /// <summary>
        /// Gets a substring of a string from end of the string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
        public static string Right(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(str.Length - len, len);
        }

        /// <summary>
        /// Uses string.Split method to split given string by given separator.
        /// </summary>
        public static string[] Split(this string str, string separator)
        {
            return str.Split(new[] {separator}, StringSplitOptions.None);
        }

        /// <summary>
        /// Uses string.Split method to split given string by given separator.
        /// </summary>
        public static string[] Split(this string str, string separator, StringSplitOptions options)
        {
            return str.Split(new[] {separator}, options);
        }

        /// <summary>
        /// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string[] SplitToLines(this string str)
        {
            return str.Split(Environment.NewLine);
        }

        /// <summary>
        /// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string[] SplitToLines(this string str, StringSplitOptions options)
        {
            return str.Split(Environment.NewLine, options);
        }

        /// <summary>
        /// Converts PascalCase string to camelCase string.
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <param name="invariantCulture">Invariant culture</param>
        /// <returns>camelCase of the string</returns>
        public static string ToCamelCase(this string str, bool invariantCulture = true)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return invariantCulture ? str.ToLowerInvariant() : str.ToLower();
            }

            return (invariantCulture ? char.ToLowerInvariant(str[0]) : char.ToLower(str[0])) + str.Substring(1);
        }

        /// <summary>
        /// Converts PascalCase string to camelCase string in specified culture.
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <param name="culture">An object that supplies culture-specific casing rules</param>
        /// <returns>camelCase of the string</returns>
        public static string ToCamelCase(this string str, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return str.ToLower(culture);
            }

            return char.ToLower(str[0], culture) + str.Substring(1);
        }

        /// <summary>
        /// Converts given PascalCase/camelCase string to sentence (by splitting words by space).
        /// Example: "ThisIsSampleSentence" is converted to "This is a sample sentence".
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <param name="invariantCulture">Invariant culture</param>
        public static string ToSentenceCase(this string str, bool invariantCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            return Regex.Replace(
                str,
                "[a-z][A-Z]",
                m => m.Value[0] + " " +
                     (invariantCulture ? char.ToLowerInvariant(m.Value[1]) : char.ToLower(m.Value[1]))
            );
        }

        /// <summary>
        /// Converts given PascalCase/camelCase string to sentence (by splitting words by space).
        /// Example: "ThisIsSampleSentence" is converted to "This is a sample sentence".
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        public static string ToSentenceCase(this string str, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1], culture));
        }

        /// <summary>
        /// Converts string to enum value.
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String value to convert</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value)
            where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return (T) Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Converts string to enum value.
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String value to convert</param>
        /// <param name="ignoreCase">Ignore case</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value, bool ignoreCase)
            where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return (T) Enum.Parse(typeof(T), value, ignoreCase);
        }

        /// <summary>
        /// Converts camelCase string to PascalCase string.
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <param name="invariantCulture">Invariant culture</param>
        /// <returns>PascalCase of the string</returns>
        public static string ToPascalCase(this string str, bool invariantCulture = true)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return invariantCulture ? str.ToUpperInvariant() : str.ToUpper();
            }

            return (invariantCulture ? char.ToUpperInvariant(str[0]) : char.ToUpper(str[0])) + str.Substring(1);
        }

        /// <summary>
        /// Converts camelCase string to PascalCase string in specified culture.
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <param name="culture">An object that supplies culture-specific casing rules</param>
        /// <returns>PascalCase of the string</returns>
        public static string ToPascalCase(this string str, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return str.ToUpper(culture);
            }

            return char.ToUpper(str[0], culture) + str.Substring(1);
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        public static string Truncate(this string str, int maxLength)
        {
            if (str == null)
            {
                return null;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            return str.Left(maxLength);
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// It adds a "..." postfix to end of the string if it's truncated.
        /// Returning string can not be longer than maxLength.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        public static string TruncateWithPostfix(this string str, int maxLength)
        {
            return TruncateWithPostfix(str, maxLength, "...");
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// It adds given <paramref name="postfix"/> to end of the string if it's truncated.
        /// Returning string can not be longer than maxLength.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        public static string TruncateWithPostfix(this string str, int maxLength, string postfix)
        {
            if (str == null)
            {
                return null;
            }

            if (str == string.Empty || maxLength == 0)
            {
                return string.Empty;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            if (maxLength <= postfix.Length)
            {
                return postfix.Left(maxLength);
            }

            return str.Left(maxLength - postfix.Length) + postfix;
        }

        private static List<string> SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
            {
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();
            }

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable()
                .Select(s => s.Trim())
                .ToList();
        }

        /// <summary>
        ///     Formats a string to an invariant culture
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="objects">The objects.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string FormatInvariant(this string format, params object[] objects)
        {
            return string.Format(CultureInfo.InvariantCulture, format, objects);
        }

        /// <summary>
        ///     Formats a string to the current culture.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="objects">The objects.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string FormatCurrent(this string format, params object[] objects)
        {
            return string.Format(CultureInfo.CurrentCulture, format, objects);
        }

        /// <summary>
        ///     Formats a string to the current UI culture.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="objects">The objects.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string FormatCurrentUi(this string format, params object[] objects)
        {
            return string.Format(CultureInfo.CurrentUICulture, format, objects);
        }

        [DebuggerStepThrough]
        public static string FormatWith(this string format, params object[] args)
        {
            return FormatWith(format, CultureInfo.CurrentCulture, args);
        }

        [DebuggerStepThrough]
        public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
        {
            return string.Format(provider, format, args);
        }

        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }

        public static bool CheckReg(this string value,string regex)
        {
            Regex expression = new Regex(regex);
            var results = expression.Match(value);
            return results.Success;

        }
    }
}