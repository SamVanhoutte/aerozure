using System.Globalization;

namespace Aerozure.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Trims the input and capitalizes the first letter character, leaving the rest of the string untouched.
    /// Leading non-letter characters (digits, quotes, …) are skipped. Idempotent: a string that already
    /// starts with an uppercase letter is returned unchanged. Null/empty/whitespace are returned as-is.
    /// </summary>
    public static string? CapitalizeFirstLetter(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        var trimmed = value.Trim();

        // Find the first letter; leave leading non-letters (digits, quotes) as-is.
        for (var i = 0; i < trimmed.Length; i++)
        {
            if (!char.IsLetter(trimmed[i]))
                continue;

            if (char.IsUpper(trimmed[i]))
                return trimmed; // already capitalized — no-op

            var upper = char.ToUpper(trimmed[i], CultureInfo.InvariantCulture);
            return trimmed[..i] + upper + trimmed[(i + 1)..];
        }

        // No letters found — nothing to capitalize.
        return trimmed;
    }

    public static string ToSystemIdentifier(this string input, char? separator = null)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        char[] charsToReplace =
            ['-', '_', ' ', '/', '?', '&', '=', '#', '%', '@', '!', '$', '^', '*', '(', ')', '+', ',', '.', ';', ':'];

        return charsToReplace.Aggregate(input,
            (current, c) => current.Replace(c.ToString(), (separator?.ToString() ?? string.Empty)));
    }
    public static bool IsValidUrl(this string? value, out Uri? uri)
    {
        uri = null;
        if (string.IsNullOrEmpty(value)) return false;
        return Uri.TryCreate(value, UriKind.Absolute, out uri)
               && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}