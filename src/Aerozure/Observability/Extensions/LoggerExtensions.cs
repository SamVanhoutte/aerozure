using System.Diagnostics;
using Aerozure.Configuration;
using Microsoft.Extensions.Logging;

namespace Aerozure.Observability.Extensions;

public static class LoggerExtensions
{
    public static void ExcludeDefaultLoglevels(this ILoggingBuilder loggingbuilder)
    {
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
    }
}