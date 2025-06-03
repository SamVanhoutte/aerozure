using System.Diagnostics;
using Aerozure.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Aerozure.Observability.Extensions;

public static class LoggerExtensions
{
    public static void ExcludeDefaultLoglevels(this ILoggingBuilder loggingbuilder)
    {
        loggingbuilder.AddFilter("AspNetCore", LogLevel.Warning);
        loggingbuilder.AddFilter("AspNetCoreEnvironment", LogLevel.Warning);
        loggingbuilder.AddFilter("Microsoft", LogLevel.Error);
        loggingbuilder.AddFilter("System", LogLevel.Error);
        loggingbuilder.AddFilter("Worker", LogLevel.Error);
        loggingbuilder.AddFilter("Host", LogLevel.Warning);
        loggingbuilder.AddFilter("Host.Startup", LogLevel.Warning);
        loggingbuilder.AddFilter("Host.Results", LogLevel.Warning);
        loggingbuilder.AddFilter("Host.Triggers.DurableTask", LogLevel.Warning);
        loggingbuilder.AddFilter("Host.Aggregator", LogLevel.Warning);
        loggingbuilder.AddFilter("Microsoft.AspNetCore", LogLevel.Error);
        loggingbuilder.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Error);
        loggingbuilder.AddFilter("Microsoft.Azure.WebJobs", LogLevel.Error);
        loggingbuilder.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Warning);
        loggingbuilder.AddFilter("Microsoft.Extensions", LogLevel.Warning);
        loggingbuilder.AddFilter("Function", LogLevel.Warning);
        loggingbuilder.AddFilter("Function.Thread", LogLevel.Warning);
        loggingbuilder.AddFilter("DurableTask.AzureStorage", LogLevel.Warning);
        loggingbuilder.AddFilter("DurableTask.Core", LogLevel.Warning);
        loggingbuilder.AddFilter("Grpc", LogLevel.Warning);
        loggingbuilder.AddFilter("Azure.Identity", LogLevel.Warning);
        loggingbuilder.AddFilter("Azure.Core", LogLevel.Warning);
        loggingbuilder.AddFilter("MSAL.NetCore", LogLevel.Error);
    }

    public static bool ShouldTrace(this HttpContext context, string[]? additionalFilters = null)
    {
        // Exclude Blazor SignalR & framework noise!
        var shouldTrace = !context.Request.Path.Value.StartsWith("/favicon.ico") &&
                          !context.Request.Path.Value.Contains("/_framework",
                              StringComparison.InvariantCultureIgnoreCase) &&
                          !context.Request.Path.Value.Contains("/_blazor",
                              StringComparison.InvariantCultureIgnoreCase) &&
                          !context.Request.Path.Value.Contains("_Host", StringComparison.InvariantCultureIgnoreCase) &&
                          !context.Request.Path.Value.EndsWith(".js", StringComparison.InvariantCultureIgnoreCase) &&
                          !context.Request.Path.Value.EndsWith(".css", StringComparison.InvariantCultureIgnoreCase) &&
                          !context.Request.Path.Value.EndsWith(".wasm", StringComparison.InvariantCultureIgnoreCase);
        if (!shouldTrace) return false;


        return !(additionalFilters?.Any(f =>
            context.Request.Path.Value.Contains(f, StringComparison.InvariantCultureIgnoreCase)) ?? false);
    }
}