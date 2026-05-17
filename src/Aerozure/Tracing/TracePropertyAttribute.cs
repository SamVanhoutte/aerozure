namespace Aerozure.Tracing
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TracePropertyAttribute(string? name = null) : Attribute
    {
        public string? Name => name;
    }
}