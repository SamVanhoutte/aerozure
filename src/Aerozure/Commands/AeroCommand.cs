using System.Data;
using Aerozure.Tracing;
using Newtonsoft.Json.Linq;

namespace Aerozure.Commands
{
    public abstract class AeroCommand : TraceRequest
    {
        public AeroCommand(string commandType)
        {
            this.CommandType = commandType.ToString();
        }

        [TraceProperty]
        public string CommandType { get; }

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