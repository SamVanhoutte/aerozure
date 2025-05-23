using System.Reflection;
using Flurl.Util;
using Namotion.Reflection;

namespace Aerozure.Tracing;

public static class TraceContextExtensionMethods
{
    public static Dictionary<string, object> GetTraceContext
    (this object? request, Dictionary<string, object> runtimeContext,
        Dictionary<string, object> customProperties)
    {
        return BuildTraceContext(runtimeContext: runtimeContext,
            customProperties: customProperties, request: request);
    }

    public static Dictionary<string, object> GetTraceContext
    (this object request, Dictionary<string, object>? runtimeContext,
        string? propertyName, object? propertyValue = null)
    {
        return BuildTraceContext(runtimeContext: runtimeContext,
            propertyName: propertyName, propertyValue:propertyValue, request: request);
    }

    public static Dictionary<string, object> GetTraceContext(
        this object? request, Dictionary<string, object> runtimeContext)
    {
        return BuildTraceContext(runtimeContext: runtimeContext, request: request);
    }


    public static Dictionary<string, object> GetTraceContext(
        this Dictionary<string, object> runtimeContext,
        string propertyName, object propertyValue)
    {
        return BuildTraceContext(runtimeContext: runtimeContext,
            propertyName: propertyName, propertyValue:propertyValue);
    }
    
    public static Dictionary<string, object> GetTraceContext(this object? request, string? propertyName, object propertyValue)
    {
        return BuildTraceContext(request: request, propertyName: propertyName, propertyValue:propertyValue);
    }

    private static Dictionary<string, object> BuildTraceContext(
        Dictionary<string, object>? runtimeContext = null,
        Dictionary<string, object>? customProperties = null,
        object? request = null,
        string? propertyName = null,
        object? propertyValue = null)
    {
        var context = new Dictionary<string, object>();
        if (runtimeContext != null) context.Merge(runtimeContext);
        if (customProperties != null) context.Merge(customProperties);
        if (request != null) context.Merge(request.GetTraceContext());
        if (!string.IsNullOrEmpty(propertyName) && propertyValue != null)
        {
            context.Add(propertyName, propertyValue);
        }

        return context;
    }

    public static Dictionary<string, object> GetTraceContext(this object? item)
    {
        if (item == null) return new Dictionary<string, object>();
        var traceContext = new Dictionary<string, object>();
        foreach (var propertyInfo in item.GetType().GetProperties())
        {
            if (propertyInfo.IsDefined(typeof(TracePropertyAttribute), true))
            {
                if (propertyInfo.IsCustomComplexType())
                {
                    var subProperty = propertyInfo.GetValue(item);
                    if (subProperty != null)
                    {
                        traceContext.Merge(subProperty.GetTraceContext());
                    }
                }
                else
                {
                    // First we check the method (deepest level), as that can override the controller
                    var attribute =
                        (TracePropertyAttribute?)propertyInfo.GetCustomAttribute(
                            typeof(TracePropertyAttribute));
                    if (attribute != null)
                    {
                        var propName = attribute.Name ?? propertyInfo.Name;
                        var propValue = propertyInfo.GetValue(item);
                        traceContext.Add(propName, propValue);
                    }
                }
            }
        }

        return traceContext;
    }

    private static bool IsCustomComplexType(this PropertyInfo propertyInfo)
    {
        var type = propertyInfo.PropertyType;

        // Handle Nullable<T>
        if (Nullable.GetUnderlyingType(type) != null)
        {
            type = Nullable.GetUnderlyingType(type);
        }

        // Exclude primitive, system types, enums, and common BCL types
        return type.IsClass || type.IsValueType
            && !type.IsPrimitive
            && type != typeof(string)
            && type != typeof(decimal)
            && type != typeof(DateTime)
            && !type.IsEnum
            && (type.Namespace == null || !type.Namespace.StartsWith("System"));
    }
}