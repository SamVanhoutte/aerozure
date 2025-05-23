// using System.Reflection;
// using System.Text.Json.Serialization;
// using Flurl.Util;
// using Namotion.Reflection;
//
// namespace Aerozure.Tracing
// {
//     public class TraceRequest
//     {
//         [JsonIgnore]
//         public Dictionary<string, object> TraceContext
//         {
//             get
//             {
//                 var traceContext = new Dictionary<string, object>();
//                 foreach (var propertyInfo in this.GetType().GetProperties())
//                 {
//                     if (propertyInfo.IsDefined(typeof(TracePropertyAttribute), true))
//                     {
//                         if (propertyInfo.PropertyType.InheritsFromTypeName(nameof(TraceRequest), TypeNameStyle.Name))
//                         {
//                             var subTraceProperty = propertyInfo.GetValue(this) as TraceRequest;
//                             if (subTraceProperty != null)
//                             {
//                                 traceContext.Merge(subTraceProperty.TraceContext);
//                             }
//                         }
//                         else
//                         {
//                             // First we check the method (deepest level), as that can override the controller
//                             var attribute =
//                                 (TracePropertyAttribute?)propertyInfo.GetCustomAttribute(
//                                     typeof(TracePropertyAttribute));
//                             if (attribute != null)
//                             {
//                                 var propName = attribute.Name ?? propertyInfo.Name;
//                                 var propValue = propertyInfo.GetValue(this);
//                                 traceContext.Add(propName, propValue);
//                             }
//                         }
//                     }
//                 }
//
//                 return traceContext;
//             }
//         }
//     }
// }