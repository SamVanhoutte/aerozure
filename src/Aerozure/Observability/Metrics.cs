namespace Aerozure.Observability;

public class Metrics
{
    public const string CommandReceived = "aerozure.commands.{0}.received";
    public const string CommandCompleted = "aerozure.commands.{0}.completed";
    public const string CommandAbandoned = "aerozure.commands.{0}.abandoned";
    public const string CommandDeadlettered = "aerozure.commands.{0}.deadlettered";
    public const string CommandDefered = "aerozure.commands.{0}.defered";
}