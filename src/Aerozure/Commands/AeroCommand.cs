using Aerozure.Tracing;
using Newtonsoft.Json.Linq;

namespace Aerozure.Commands
{
    public abstract class AeroCommand(string commandType)
    {
        [TraceProperty]
        public string CommandType { get; } = commandType.ToString();

        public string ToJson()
        {
            var jo = JObject.FromObject(this);
            return jo.ToString();
        }

        public virtual string GenerateSessionId()
        {
            return Guid.NewGuid().ToString();
        }

        public abstract string GenerateMessageId();
    }
}