namespace Aerozure.Tracing
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TracePropertyAttribute : Attribute
    {
        public TracePropertyAttribute()
        {
            
        }

        public TracePropertyAttribute(string name)
        {
            Name = name;
        }
        public string? Name { get; set; }        
    }
}