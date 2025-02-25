using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Unisphere.Core.Common.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Capitalize a string (the first char is a upper char and the rest is in lower).
    /// </summary>
    /// <param name="text">The not-null input string.</param>
    /// <returns>The input string in capitalized.</returns>
    /// <exception cref="ArgumentNullException">If the text provided is null.</exception>
    public static string Capitalize(this string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        return char.ToUpperInvariant(text[0]) + text.ToLowerInvariant().Substring(1);
    }

    /// <summary>
    /// Remove accentuation.
    /// </summary>
    /// <param name="text">a string with or without accent.</param>
    /// <returns>a string without accent.</returns>
    public static string RemoveDiacritics(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (char c in normalizedString)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Truncates a string to the specified length.
    /// </summary>
    /// <param name="value">The string to be truncated.</param>
    /// <param name="length">The maximum length.</param>
    /// <returns>Truncated string.</returns>
    public static string Truncate(this string value, int length) => Truncate(value, length, false);

    /// <summary>
    /// Truncates a string to the specified length.
    /// </summary>
    /// <param name="value">The string to be truncated.</param>
    /// <param name="length">The maximum length.</param>
    /// <param name="ellipsis"><c>true</c> to add ellipsis to the truncated text; otherwise, <c>false</c>. The final size will then be (length + 3).</param>
    /// <returns>Truncated string.</returns>
    public static string Truncate(this string value, int length, bool ellipsis)
    {
        if (!string.IsNullOrEmpty(value))
        {
            value = value.Trim();
            if (value.Length > length)
            {
                if (ellipsis)
                {
                    return string.Concat(value.AsSpan(0, length - 3), "...");
                }

                return value.Substring(0, length);
            }
        }

        return value ?? string.Empty;
    }

    public static string TruncateKeepExtension(this string value, int length)
    {
        var dotIndex = value.LastIndexOf('.');

        if (dotIndex == -1)
        {
            return value.Truncate(length);
        }

        string extension = value.Substring(dotIndex);
        string baseName = value.Substring(0, dotIndex);

        return baseName.Truncate(length - extension.Length) + extension;
    }

    public static Stream ToStream(this string source) => new MemoryStream(Encoding.UTF8.GetBytes(source));

    public static IEnumerable<string> SplitToLogicString(this string source, int maxLenght = 35, char loqicSeparator = ' ')
    {
        var parts = new List<string>();

        if (source.Length <= maxLenght)
        {
            parts.Add(source);

            return parts;
        }

        var sourceSplitted = source.Split(loqicSeparator, StringSplitOptions.RemoveEmptyEntries);

        if (sourceSplitted.Length == 1)
        {
            parts.Add(source.Substring(0, maxLenght));

            return parts;
        }

        var partialStr = sourceSplitted[0];
        var i = 1;
        var isLastAdded = false;

        while (i < sourceSplitted.Length)
        {
            var combine = partialStr + loqicSeparator + sourceSplitted[i];
            if (combine.Length > maxLenght)
            {
                parts.Add(partialStr);
                partialStr = sourceSplitted[i];
                isLastAdded = true;
                if (i == sourceSplitted.Length - 1)
                {
                    parts.Add(partialStr);
                }
            }
            else
            {
                partialStr = combine;
                isLastAdded = false;
            }

            i++;
        }

        if (!isLastAdded)
        {
            parts.Add(partialStr);
        }

        return parts;
    }

    public static string MentionName(string firstName, string lastName)
    {
        return $"@{firstName.Replace(" ", "-", StringComparison.OrdinalIgnoreCase)}_{lastName.Replace(" ", "-", StringComparison.OrdinalIgnoreCase).ToUpperInvariant()} ";
    }

    /// <summary>
    /// Find all occurrences respecting this format @FirstnameLASTNAME.
    /// </summary>
    /// <param name="source">Find all firstnames with lastnames.</param>
    /// <param name="separator">Seperator between firstname and name. Can't be an empty string.</param>
    /// <param name="removeDiacritics">Remove Diacritics or not.</param>
    /// <returns>List of lastname with firstname.</returns>
    public static IList<(string FirstName, string LastName)> MentionedNames(this string source, string separator = "_", bool removeDiacritics = false)
    {
        return new Regex(GetMentionedNamesPattern(separator, removeDiacritics))
            .Matches(removeDiacritics ? source.RemoveDiacritics() : source)
            .Select(match => (match.Groups[1].Value, match.Groups[2].Value))
            .Distinct()
            .ToList();
    }

    /// <summary>
    /// Remove occurrences respecting this format @FirstnameLASTNAME.
    /// </summary>
    /// <param name="source">Find all firstnames with lastnames.</param>
    /// <param name="separator">Seperator between firstname and name. Can't be an empty string.</param>
    /// <param name="removeDiacritics">Remove Diacritics or not.</param>
    /// <returns>Source without mentions.</returns>
    public static string RemoveMentionedNames(this string source, string separator = "_", bool removeDiacritics = false)
    {
        return new Regex(GetMentionedNamesPattern(separator, removeDiacritics)).Replace(source, string.Empty).Replace("  ", " ", StringComparison.InvariantCulture);
    }

    private static string GetMentionedNamesPattern(string separator = "_", bool removeDiacritics = false)
    {
        if (removeDiacritics)
        {
            return $"@([A-Z][a-z-]+){separator}([A-Z-]+)";
        }
        else
        {
            var lowerCharsAccented = "àèìòùáéíóúýâêîôûäëïöüÿçñ";
            var upperCharsAccented = "ÀÈÌÒÙÁÉÍÓÚÝÂÊÎÔÛÄËÏÖÜŸÇÑ";

            return $"@([A-Za-z{upperCharsAccented}{lowerCharsAccented}-]+){separator}([A-Z{upperCharsAccented}-]+)";
        }
    }

    public static bool IsBase64String(this string base64)
    {
        Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
        return Convert.TryFromBase64String(base64, buffer, out int _);
    }

    private static readonly Regex HttpUrlRregex = new Regex("(http(s)?):\\/\\/.\\S+", RegexOptions.Compiled);

    public static IList<string> ExtractUrls(this string source, out string replacedSource, string? replacement = null)
    {
        var punctuationCharacters = new[] { '.', '?', '!', ',', ';', ':' };

        var matches = HttpUrlRregex.Matches(source).Select(m => m.Value).ToList();
        replacedSource = source;

        foreach (var match in matches.OrderByDescending(m => m.Length))
        {
            var i = matches.IndexOf(match);

            // Remove punctuation at the end of url if there is any
            if (punctuationCharacters.Contains(match[^1]))
            {
                matches[i] = match.TrimEnd(match[^1]);
            }

            replacedSource = replacedSource.Replace(matches[i], replacement, StringComparison.InvariantCulture);
        }

        return matches;
    }

    private static readonly Regex HtmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
    private static readonly Regex BrRegex = new Regex(@"(<br>|<br />|<br/>|</ br>|</br>)", RegexOptions.Compiled);

    public static string RemoveHtmlTags(this string html, bool keepNewLine = true)
    {
        if (keepNewLine)
        {
            html = html.Replace("</p>", "NewLine", StringComparison.InvariantCulture);
            html = BrRegex.Replace(html, "NewLine");
        }

        var result = HtmlRegex.Replace(HttpUtility.HtmlDecode(html), string.Empty);

        if (keepNewLine)
        {
            result = result.Replace(" NewLine ", "NewLine", StringComparison.InvariantCulture)
                .Replace(" NewLine", "NewLine", StringComparison.InvariantCulture)
                .Replace("NewLine ", "NewLine", StringComparison.InvariantCulture)
                .Replace("NewLine", Environment.NewLine, StringComparison.InvariantCulture);
        }

        return result
            .Replace("  ", " ", StringComparison.InvariantCulture)
            .Replace("\u00A0", " ", StringComparison.InvariantCulture)
            .Trim();
    }

    public static string RemoveNewLine(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return text;
        }

        text = text.Replace("\r\n", " ", StringComparison.InvariantCulture);
        text = text.Replace("\n", " ", StringComparison.InvariantCulture);
        text = text.Replace("\r", " ", StringComparison.InvariantCulture);

        return text.Trim();
    }

    public static string Sha256(this string value)
    {
        StringBuilder builder = new StringBuilder();

        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));

        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2", CultureInfo.InvariantCulture));
        }

        return builder.ToString();
    }

    public static string ReplaceFirst(this string text, string search, string replace, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
    {
        int pos = text.IndexOf(search, stringComparison);

        if (pos < 0)
        {
            return text;
        }

        return string.Concat(text.AsSpan(0, pos), replace, text.AsSpan(pos + search.Length));
    }

    public static Guid AsGuid(this string guidValue)
    {
        ArgumentNullException.ThrowIfNull(guidValue);

        return new Guid(guidValue);
    }
}
