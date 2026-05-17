using Aerozure.Tracing;
using Newtonsoft.Json.Linq;

namespace Aerozure.Commands
{
    
    public abstract class AeroEvent(string eventType)
    {
        [TraceProperty]
        public string EventType { get; } = eventType.ToString();

        [TraceProperty]
        public abstract string MessageId { get; }
        public string ToJson()
        {
            var jo = JObject.FromObject(this);
            return jo.ToString();
        }
    }
}