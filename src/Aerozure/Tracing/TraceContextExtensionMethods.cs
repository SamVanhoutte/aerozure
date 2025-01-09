using System.Collections.Generic;
using Aerozure.Tracing;
using Flurl.Util;

namespace Sfinx.Backend.Core.Observability;

public static class TraceContextExtensionMethods
{
    public static Dictionary<string, object> GetTraceContext(this Dictionary<string, object> runtimeContext, Dictionary<string, object> customProperties,
        TraceRequest request = null)
    {
        customProperties.Merge(runtimeContext);
        if (request != null)
        {
            customProperties.Merge(request.TraceContext);
        }
        
        return customProperties;
    }
    
    public static Dictionary<string, object> GetTraceContext(this Dictionary<string, object> runtimeContext, TraceRequest request, string propertyName = null,
        object propertyValue = null)
    {
        var requestContext = request.TraceContext;
        if (!string.IsNullOrEmpty(propertyName) && propertyValue != null)
        {
            requestContext.Add(propertyName, propertyValue);
        }
        
        return runtimeContext.GetTraceContext(requestContext);
    }

    public static TraceContext SetTraceContext(this Dictionary<string, object> runtimeContext, TraceRequest request, string name, object value)
    {
        runtimeContext.Add(name, value);
        runtimeContext.Merge(request.TraceContext);
        return new TraceContext(runtimeContext);
    }
    
    public static TraceContext SetTraceContext(this Dictionary<string, object> runtimeContext,  Dictionary<string, object> customContext,TraceRequest request)
    {
        runtimeContext.Merge(customContext);
        runtimeContext.Merge(request.TraceContext);
        return new TraceContext(runtimeContext);
    }
    
    public static TraceContext SetTraceContext(this Dictionary<string, object> runtimeContext,  TraceRequest request)
    {
        runtimeContext.Merge(request.TraceContext);
        return new TraceContext(runtimeContext);
    }
    public static TraceContext SetTraceContext(this Dictionary<string, object> runtimeContext, string name, object value)
    {
        runtimeContext.Add(name, value);
        return new TraceContext(runtimeContext);
    }
    
    public static TraceContext SetTraceContext(this Dictionary<string, object> runtimeContext, Dictionary<string, object> customContext)
    {
        customContext.Merge(runtimeContext);
        return new TraceContext(customContext);
    }
    
    public static TraceContext SetTraceContext(this Dictionary<string, object> runtimeContext)
    {
        return new TraceContext(runtimeContext);
    }
    
    
        
    public static TraceContext SetTraceContext(this TraceRequest request, string propertyName = null,
        object propertyValue = null)
    {
        var requestContext = request.TraceContext;
        if (!string.IsNullOrEmpty(propertyName) && propertyValue != null)
        {
            requestContext.Add(propertyName, propertyValue);
        }
        
        return SetTraceContext(requestContext);
    }


    public static Dictionary<string, object> GetTraceContext(this Dictionary<string, object> runtimeContext , string name, object value)
    {
        runtimeContext.Add(name, value);
        return runtimeContext;
    }

}