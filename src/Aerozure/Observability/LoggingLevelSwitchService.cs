// using Serilog.Core;
// using Serilog.Events;
//
// namespace Aerozure.Observability;
//
// public class LoggingLevelSwitchService : ILoggingLevelSwitchService
// {
//     private readonly LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch();
//
//     public LoggingLevelSwitch LevelSwitch => levelSwitch;
//
//     public void SetLogLevel(LogEventLevel level)
//     {
//         levelSwitch.MinimumLevel = level;
//     }
// }