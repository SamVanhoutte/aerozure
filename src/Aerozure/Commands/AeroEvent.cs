using Aerozure.Tracing;
using Newtonsoft.Json.Linq;

namespace Aerozure.Commands
{
    
    public abstract class AeroEvent : TraceRequest
    {
        public AeroEvent(string eventType)
        {
            this.EventType = eventType.ToString();
        }

        [TraceProperty]
        public string EventType { get; }

        [TraceProperty]
        public abstract string MessageId { get; }
        public string ToJson()
        {
            var jo = JObject.FromObject(this);
            return jo.ToString();
        }
    }
}