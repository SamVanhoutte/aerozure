namespace Aerozure.Extensions
{

    public static class TimeSpanExtensionsMethods
    {
        public static bool IsLongerThan(this TimeSpan instance, TimeSpan other)
        {
            return instance.CompareTo(other) > 0;
        }

        public static TimeSpan WholeMinutes(this TimeSpan instance)
        {
            var totalSeconds = instance.TotalSeconds;
            var remainingSeconds = instance.TotalSeconds % 60;
            var result = TimeSpan.FromSeconds(totalSeconds - remainingSeconds);
            return result;
        }
    }
}