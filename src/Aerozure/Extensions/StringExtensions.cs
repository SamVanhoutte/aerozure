namespace Aerozure.Extensions;

public static class StringExtensions
{
    public static string ToSystemIdentifier(this string input, char? separator = null)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        char[] charsToReplace =
            ['-', '_', ' ', '/', '?', '&', '=', '#', '%', '@', '!', '$', '^', '*', '(', ')', '+', ',', '.', ';', ':'];

        return charsToReplace.Aggregate(input,
            (current, c) => current.Replace(c.ToString(), (separator?.ToString() ?? string.Empty)));
    }
    
}