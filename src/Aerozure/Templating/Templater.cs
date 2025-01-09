using HandlebarsDotNet;

namespace Aerozure.Templating;

public static class Templater
{
    public static async Task<string> ParseAsync(string source, object structuredData)
    {
        var template = Handlebars.Compile(source);
        return template(structuredData);
    }
}